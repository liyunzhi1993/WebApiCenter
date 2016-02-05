using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;

namespace WebApiCenter.Models.Common
{
    public enum FormatType
    {
        XML,
        JSON,
        HTML
    }

    public enum ApplicationType
    {
        WEB = 1,
        DESKTOP = 2
    }

    public enum ErrorType
    {
        #region Member_login Error

        User_TimeOut_Error = 304, // 用户过期
        User_Not_Exist = 305, // 用户不存在

        #endregion

        #region Data Error
        Data_Not_Exist = 900, // 请求数据不存在
        #endregion


        #region System Error

        API_EC_UNKNOWN = 1, //发生未知错误,请重新提交请求
        API_EC_SERVICE = 2, //服务现在不可用
        API_EC_METHOD = 3, //未知方法
        API_EC_TOO_MANY_CALLS = 4, //已到达最大请求
        API_EC_CLASS = 30, //未知访问类
        API_EC_PARAM = 100, //指定的参数之一丢失或无效
        API_EC_CALLID = 103, //提交的call_id不大于该会话的前call_id
        #endregion

        #region Authrity Error

        API_EC_BAD_IP = 5, //请求来自于该应用程序不允许的远程地址
        API_EC_NO_API = 6,//没有对应的API
        API_EC_NO_API_ACCESS = 7,//没有访问该API的权限
        API_EC_PERMISSION_DENIED = 10, //应用程序没有此操作权限 
        API_EC_APPLICATION = 101, //API没有与任何已知的应用程序关联的
        API_EC_SESSIONKEY = 102, //Session是不正确提交或已超时,用户直接登录以获得另一个密钥
        API_EC_SIGNATURE = 104, //错误签名

        #endregion

        #region Application Error

        API_EC_REGISTER_NOT_ALLOW = 109,//不允许注册

        API_EC_USER_ALREADY_EXIST = 110,//用户名已经存在
        API_EC_USERNAME_ILLEGAL = 112,//不允许的用户名
        API_EC_USER_ONLINE = 113,//用户已经在线

        #endregion
    }
}
