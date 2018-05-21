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
    /// this is a authroize attribute this is placed on top of controller methods and will validate 
    /// </summary>
    public class AdminUser : AuthorizeAttribute
    {
        /// <summary>
        /// this will return true or false depending on whether the employee has admin priveldges or not this will then be handled by the method below.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                return CurrentUser.EmployeeLogin.Employee.Admin;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// this method shall be called when a user does not have the correct priveldges. They will be
        /// redirected to the login screen if they do not.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
        }
    }
}