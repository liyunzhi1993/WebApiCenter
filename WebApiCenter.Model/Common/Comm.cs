using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCenter.Models.Common
{
    /// <summary>
    /// Comm
    /// </summary>
    /// 创建人：李允智
    /// 创建时间：2016/1/28
    /// 描述：返回成功/失败 以及消息..
    public class Comm
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
