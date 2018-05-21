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
    /// <summary>
    /// This again is a attribute that will asses when a request comes into a controller or a method whether the user
    /// currently has a sessions running. If they do not it will redirect them to another page.
    /// </summary>
    public class NormalUser : AuthorizeAttribute
    {
        /// <summary>
        /// this method returns a boolean and will state if the user has a sessions running.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
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

        /// <summary>
        /// this will be called if the return from the above method is false and it will redirect the user to the
        /// login screen.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Account", action = "Index"}));
        }
    }
}