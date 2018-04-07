using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Garden_Centre_MVC.Assets;
using Garden_Centre_MVC.Attributes.Assets;

namespace Garden_Centre_MVC.Attributes
{
    public class NormalUser : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                return httpContext.Session[CurrentUser.EmployeeLogin.Username] != null;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Account", action = "Index"}));
        }
    }
}