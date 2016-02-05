using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCenter.Models.Client
{
    /// <summary>
    /// User
    /// </summary>
    /// 创建人：李允智
    /// 创建时间：2016/1/27
    /// 描述：登录用户的表
    public class Users : BaseModel
    {
        public string account { get; set; }
        public string password { get; set; }
    }
}