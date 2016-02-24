using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using WebApiCenter.Core.Client;
using WebApiCenter.Core.Common;
using WebApiCenter.Models.Client;
using WebApiCenter.Models.Common;

namespace WebApiCenter.App_Start
{
    public class ApiHttpHandler : IHttpHandler
    {
        /*---- 局部变量 ----*/
        private HttpContext currentContext = null;
        private string apiKey = "";
        private string secretKey = "";
        private LMSParam[] parameters = null;

        /*---- 必选参数 ----*/
        string api_key = String.Empty;
        string method = String.Empty;
        string sig = String.Empty;
        string api_id = string.Empty;

        /*---- 可选参数 ----*/
        string format = "JSON";

        /*---- 初始化操作变量 ----*/
        private ClientsCore clientCore = new ClientsCore();
        private AuthorizationsCore authorizationsCore = new AuthorizationsCore();
        private ApiLibraryCore apiLibraryCore = new ApiLibraryCore();

        /*---- 返回数据 ----*/
        private ReturnMsg returnMsg = new ReturnMsg();
        public void ProcessRequest(HttpContext context)
        {
            this.currentContext = context;
            context.Response.Clear();
            context.Response.ContentType = "text/html";

            parameters = GetParamsFromRequest(context.Request);

            try
            {
                /*---- 获取可选参数 ----*/
                object o = GetParam("format");
                if (o != null && o.ToString().Trim() != "")
                    format = o.ToString().Trim();

                /*---- 校验必选参数 ----*/ 
                o = GetParam("api_key");
                if (o != null && o.ToString().Trim() != "")
                    api_key = o.ToString().Trim();
                else
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_APPLICATION);
                    return;
                }

                //检查是否上传了api_id
                o = GetParam("api_id");
                if (o != null && o.ToString().Trim() != "")
                    api_id = o.ToString().Trim();
                else
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_NO_API);
                    return;
                }

                //根据api_key去mongodb去查,先查这个是否存在的api_key
                Clients client=clientCore.GetClientByClientid(api_key);
                if (client == null)
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_APPLICATION);
                    return;
                }
                else
                {
                    apiKey = client.clientid;
                    secretKey = client.clientsecret;
                }

                //判断状态是否正常 如果暂停则返回服务暂不可用
                if (!client.isenabled)
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_SERVICE);
                    return;
                }

                //如果存在api_key 那么在查询是否有接入ip限制
                if (!string.IsNullOrEmpty(client.reqip))
                {
                    if (GetIp()!=client.reqip)
                    {
                        ResponseErrorInfo((int)ErrorType.API_EC_BAD_IP);
                        return;
                    }
                }

                //查询是否赋予了查询该Api的权限
                Authorizations authorization=authorizationsCore.GetAuthorization(client.id);
                if (authorization == null)
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_NO_API_ACCESS);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(authorization.apilibraryids))
                    {
                        ResponseErrorInfo((int)ErrorType.API_EC_NO_API_ACCESS);
                        return;
                    }
                    else
                    {
                        if (!authorization.apilibraryids.Contains(api_id))
                        {
                            ResponseErrorInfo((int)ErrorType.API_EC_NO_API_ACCESS);
                            return;
                        }
                    }
                }

                o = GetParam("method");
                if (o != null && o.ToString().Trim() != "")
                    method = o.ToString().Trim();
                else
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_METHOD);
                    return;
                }

                o = GetParam("sig");
                if (o != null && o.ToString().Trim() != "")
                    sig = o.ToString().Trim();
                else
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_SIGNATURE);
                    return;
                }

                string sign = GetSignature(parameters, secretKey);
                if (sign.ToLower() != sig.ToLower())
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_SIGNATURE);
                    return;
                }
            }
            catch
            {
                ResponseErrorInfo((int)ErrorType.API_EC_PARAM);
                return;
            }

            string classname = method.Substring(0, method.LastIndexOf('.'));
            string methodname = method.Substring(method.LastIndexOf('.') + 1);

            string content = String.Empty;
            ActionBase action;

            try
            {
                ApiLibrary apiLibrary = apiLibraryCore.GetApiLibrary(api_id);
                Assembly t = Assembly.Load("WebApiCenter."+apiLibrary.apiname);
                Type typeInstance=null;
                foreach (Type type in t.GetTypes())
                {
                    if (type.Name==classname)
                    {
                        typeInstance = type;
                        break;
                    }
                }
                if (typeInstance == null)
                {
                    ResponseErrorInfo((int)ErrorType.API_EC_METHOD);
                    return;
                }

                action = (ActionBase)Activator.CreateInstance(typeInstance);
                action.ApiKey = apiKey;
                action.Params = parameters;
                action.Secret = secretKey;
                action.Format = FormatType.JSON;
                context.Response.ContentType = "application/Json";
                action.Signature = sig;

                if (format.Trim().ToLower() == "xml")
                {
                    context.Response.ContentType = "text/xml";
                    action.Format = FormatType.XML; 
                }

                content = typeInstance.InvokeMember(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase, null, action, new object[] { }).ToString();
            }
            catch (Exception ex)
            {
                content = String.Empty;
                ResponseErrorInfo((int)ErrorType.API_EC_METHOD);
                return;
            }
            if (action.ErrorCode > 0)
            {
                content = String.Empty;
                ResponseErrorInfo(action.ErrorCode);
                return;
            }
            this.currentContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            this.currentContext.Response.Write(content);
            this.currentContext.ApplicationInstance.CompleteRequest();
        }

        private void ResponseErrorInfo(int errorCode)
        {
            returnMsg.code = errorCode;
            string responseStr = string.Empty;
            if (format == "" || format.ToLower() == "json")
            {
                this.currentContext.Response.ContentType = "text/html";
                responseStr = JsonConvert.SerializeObject(returnMsg);
            }
            else if (format.ToLower() == "xml")
            {
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(returnMsg));
                responseStr = JsonConvert.SerializeXmlNode(doc); 
            }
            else//转成html
            {
                this.currentContext.Response.ContentType = "text/xml";
                responseStr = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(returnMsg), "root").OuterXml;
            }
            this.currentContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            this.currentContext.Response.Write(responseStr);
            this.currentContext.ApplicationInstance.CompleteRequest();

        }
        /// <summary>  
        /// 获取请求Ip  
        /// </summary>  
        /// <returns></returns>  
        public String GetIp()
        {
            String IP = "";
            if (System.Web.HttpContext.Current != null)
            {
                IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(IP) || (IP.ToLower() == "unknown"))
                {
                    IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
                    if (string.IsNullOrEmpty(IP))
                    {
                        IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    IP = IP.Split(',')[0];
                }
            }
            return IP;
        }  

        /// <summary>
        /// Gets the parameters from request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：获取API提交的参数
        private LMSParam[] GetParamsFromRequest(HttpRequest request)
        {
            List<LMSParam> list = new List<LMSParam>();
            foreach (string key in request.QueryString.AllKeys)
            {
                list.Add(LMSParam.Create(key, request.QueryString[key]));
            }
            foreach (string key in request.Form.AllKeys)
            {
                list.Add(LMSParam.Create(key, request.Form[key]));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：根据参数和密码生成签名字符串
        private string GetSignature(LMSParam[] parameters, string secretKey)
        {
            StringBuilder values = new StringBuilder();

            foreach (LMSParam param in parameters)
            {
                if (param.Name.ToLower() == "sig")
                    continue;
                if (values.ToString() == "")
                    values.Append(param.ToString());
                else
                    values.Append("&" + param.ToString());
            }
            values.Append("&" + secretKey);

            return SHA1(System.Web.HttpUtility.UrlDecode(values.ToString()));
        }

        /// <summary>
        /// Shes the a1.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：SHA1 加密函数 
        public static string SHA1(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：获取参数
        private string GetParameters(LMSParam[] parameters)
        {
            StringBuilder values = new StringBuilder();

            foreach (LMSParam param in parameters)
            {
                if (string.IsNullOrEmpty(param.Value))
                    continue;
                if (values.ToString() == "")
                    values.Append(param.ToString());
                else
                    values.Append("&" + param.ToString());
            }

            return System.Web.HttpUtility.UrlDecode(values.ToString());
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：获取参数
        private object GetParam(string key)
        {
            if (parameters == null)
                return null;
            foreach (LMSParam p in parameters)
            {
                if (p.Name.ToLower() == key.ToLower())
                {
                    return p.Value;
                }
            }
            return null;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}