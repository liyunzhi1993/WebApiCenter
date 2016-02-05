using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCenter.Models.Client
{
    public class ApiLibrary : BaseModel
    {
        public string apiname { get; set; }//api名称
        public string description { get; set; }//描述
        public string docurl { get; set; }//文档url
    }
}
