using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Assets;

namespace Garden_Centre_MVC.Attributes
{
    public class AdminUser : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return CurrentUser.EmployeeLogin.Employee.Admin;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result= new HttpNotFoundResult("You are not an admin user please log out and log in as a admin user");
        }
    }
}