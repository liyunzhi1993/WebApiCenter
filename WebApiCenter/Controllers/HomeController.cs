using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApiCenter.Core.Client;
using WebApiCenter.Core.Common;
using WebApiCenter.Models.Client;
using WebApiCenter.Models.Common;

namespace WebApiCenter.Controllers
{
    public class HomeController : Controller
    {
        private UsersCore usersCore = new UsersCore();
        private Comm comm = new Comm();
        //
        // GET: /Home/

        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/28
        /// 描述：登录
        public string Login(Users user)
        {
            if (usersCore.Login(user) != null)
            {
                FormsAuthentication.SetAuthCookie(user.account, false);
                comm.success = true;
                return JsonHelper.SerializeObject(comm);
            }
            else
            {
                comm.success = false;
                comm.message = "登录失败，请检查账号密码";
                return JsonHelper.SerializeObject(comm);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        /// 创建人：李允智
        /// 创建时间：2016/1/28
        /// 描述：登出
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
