using System;
using System.Collections.Generic;
using System.Text;
using WebApiCenter.Models.Common;

namespace WebApiCenter.Core.Common
{
    public abstract class ActionBase
    {
        private Guid uid = Guid.Empty;
        private string secret;
        private string api_key;
        private FormatType format;
        private LMSParam[] parameters;
        private int error_code;
        private double last_call_id;
        private double call_id;
        private string signature;

        public Guid Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }

        public string ApiKey
        {
            get { return api_key; }
            set { api_key = value; }
        }

        public FormatType Format
        {
            get { return format; }
            set { format = value; }
        }

        public LMSParam[] Params
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public int ErrorCode
        {
            get { return error_code; }
            set { error_code = value; }
        }

        public double LastCallId
        {
            get { return last_call_id; }
            set { last_call_id = value; }
        }

        public double CallId
        {
            get { return call_id; }
            set { call_id = value; }
        }

        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }


        /// <summary>
        /// 获得参数对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object GetParam(string key)
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

        /// <summary>
        /// 检查需要的参数是否存在
        /// </summary>
        /// <param name="paramArray">参数数组字符串</param>
        /// <returns></returns>
        public bool CheckRequiredParams(string paramArray)
        {
            string[] parms = paramArray.Split(',');
            for (int i = 0; i < parms.Length; i++)
            {
                if (GetParam(parms[i]) == null || GetParam(parms[i]).ToString().Trim() == string.Empty)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断参数是否为空或者为0
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool AreParamsNullOrZeroOrEmptyString(params object[] parms)
        {
            foreach (object obj in parms)
            {
                if (obj == null)
                {
                    return true;
                }

                if (obj.GetType() == typeof(int) && Convert.ToInt32(obj) == 0)
                {
                    return true;
                }

                if (obj.GetType() == typeof(string) && obj.ToString() == string.Empty)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
