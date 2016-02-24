using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApiCenter.Core;
using WebApiCenter.Core.Client;
using WebApiCenter.Core.Common;
using WebApiCenter.Models.Client;
using WebApiCenter.Models.Common;

namespace WebApiCenter.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private Comm comm = new Comm();
        //
        // GET: /Client/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the client list.
        /// </summary>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/28
        /// 描述：获取所有客户端列表
        public string GetClientList()
        {
            return JsonHelper.SerializeObject(ClientsCore.GetInstance().GetClientList());
        }

        /// <summary>
        /// Gets the API list.
        /// </summary>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：获取所有API列表
        public string GetApiList()
        {
            return JsonHelper.SerializeObject(ApiLibraryCore.GetInstance().GetApiLibraryList());
        }

        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <param name="clientsid">The clientsid.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：获取API权限
        public string GetAuthorization(string clientsid)
        {
            return JsonHelper.SerializeObject(AuthorizationsCore.GetInstance().GetAuthorization(clientsid));
        }

        /// <summary>
        /// Saves the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/28
        /// 描述：保存一个客户端
        public string Save(Clients client,string id,bool isnew)
        {
            try
            {
                if (isnew)
                {
                    client.isenabled = true;
                    client.createtime = DateTime.Now;
                    ClientsCore.GetInstance().InsertClient(client);
                }
                else
                {
                    Clients editClient = ClientsCore.GetInstance().GetClient(id);
                    editClient.clientname = client.clientname;
                    editClient.reqip = client.reqip;
                    ClientsCore.GetInstance().UpdateClient(editClient);
                }
                comm.success = true;
                comm.message = "保存成功";
            }
            catch (Exception)
            {
                comm.success = false;
                comm.message = "保存失败";
            }
            return JsonHelper.SerializeObject(comm);
        }

        /// <summary>
        /// Inserts the authorization.
        /// </summary>
        /// <param name="clientsid">The clientsid.</param>
        /// <param name="apilibraryids">The apilibraryids.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/2
        /// 描述：插入权限
        public string InsertAuthorization(string clientsid, string apilibraryids)
        {
            try
            {
                Authorizations authorization = AuthorizationsCore.GetInstance().GetAuthorization(clientsid);
                if (string.IsNullOrEmpty(apilibraryids))
                {
                    AuthorizationsCore.GetInstance().DeleteAuthorization(authorization.id);
                }
                else
                {
                    if (authorization != null)
                    {
                        AuthorizationsCore.GetInstance().DeleteAuthorization(authorization.id);
                    }
                    authorization = new Authorizations();
                    authorization.apilibraryids = apilibraryids;
                    authorization.clientsid = clientsid;
                    AuthorizationsCore.GetInstance().InsertAuthorization(authorization);
                }
                comm.success = true;
                comm.message = "保存成功";
            }
            catch (Exception)
            {
                comm.success = false;
                comm.message = "保存失败";
            }
            return JsonHelper.SerializeObject(comm);
        }

        /// <summary>
        /// Uploads the specified apiid.
        /// </summary>
        /// <param name="apiid">The apiid.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/4
        /// 描述：上传文档
        [HttpPost]
        public string Upload(string apiid)
        {
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    Random rnd = new Random();
                    string path = "";
                    HttpPostedFile file = files[0];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string extension = Path.GetExtension(fileName);
                        int num = rnd.Next(5000, 1000000);
                        path = "/Data/" + num.ToString() + extension;
                        file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));
                    }
                    ApiLibrary apiLibrary = ApiLibraryCore.GetInstance().GetApiLibrary(apiid);
                    apiLibrary.docurl = path;
                    ApiLibraryCore.GetInstance().UpdateApiLibrary(apiLibrary);
                    comm.success = true;
                    comm.message = "上传成功";
                }
            }
            catch (Exception)
            {
                comm.success = false;
                comm.message = "上传失败";
            }
            return JsonHelper.SerializeObject(comm);
        }

        /// <summary>
        /// Deletes the specified _id.
        /// </summary>
        /// <param name="_id">The _id.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/28
        /// 描述：删除一个客户端
        public string Del(string id)
        {
            try
            {
                ClientsCore.GetInstance().Del(id);
                comm.success = true;
                comm.message = "删除成功";
            }
            catch (Exception)
            {
                comm.success = false;
                comm.message = "删除失败";
            }
            return JsonHelper.SerializeObject(comm);
        }

        /// <summary>
        /// Pauses the specified clientsecret.
        /// </summary>
        /// <param name="clientsecret">The clientsecret.</param>
        /// <param name="ispause">if set to <c>true</c> [ispause].</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/28
        /// 描述：暂停或者激活正常
        public string ChangeStatus(string id)
        {
            try
            {
                Clients client = ClientsCore.GetInstance().GetClient(id);
                client.isenabled = !client.isenabled;
                ClientsCore.GetInstance().UpdateClient(client);
                comm.success = true;
                comm.message = "操作成功";
            }
            catch (Exception)
            {
                comm.success = false;
                comm.message = "操作失败";
            }
            return JsonHelper.SerializeObject(comm);
        }

        /// <summary>
        /// Creates the secret.
        /// </summary>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/29
        /// 描述：生成APP ID APP Secret
        public string CreateSecret()
        {
            ClientSecret clientSecret = new ClientSecret();
            clientSecret.clientid = ComHelper.GetGuid();
            clientSecret.clientsecret = ComHelper.GetSecret();
            return JsonHelper.SerializeObject(clientSecret);
        }

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/2/4
        /// 描述：进行测试
        public string RunTest(string method,string paramsStr,string clientid,string clientsecret,string url,string apiid)
        {
            string appkey = clientid;
            string appsecret = clientsecret;
            string sign = SHA1("method=" + method + "&api_key=" + appkey + paramsStr +"&api_id=" + apiid+"&"+ appsecret).ToUpper();
            string p = "method=" + method + "&api_key=" + appkey + "&sig=" + sign + paramsStr + "&api_id=" + apiid;
            return ComHelper.HttpPost(url, p);
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
    }
}
