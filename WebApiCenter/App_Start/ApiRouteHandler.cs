using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApiCenter.App_Start
{
    public class ApiRouteHandler : IRouteHandler
    {

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ApiHttpHandler();
        }
    }
}