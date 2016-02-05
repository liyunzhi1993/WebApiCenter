using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCenter.Models.Client
{
    public class Clients : BaseModel
    {
        public string clientname { get; set; }
        public string clientid { get; set; }//api_key
        public string clientsecret { get; set; }//api_secret
        //请求IP
        public string reqip { get; set; }
        public DateTime createtime { get; set; }
        public bool isenabled { get; set; }
    }
}
