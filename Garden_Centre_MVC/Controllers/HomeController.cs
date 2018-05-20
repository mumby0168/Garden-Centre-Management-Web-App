using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Attributes;

namespace Garden_Centre_MVC.Controllers
{
    /// <summary>
    /// this controller will load the home page as well as handling any requests to this  page.
    /// </summary>
    [NormalUser]
    public class HomeController : Controller
    {
        /// <summary>
        /// this method will load the home page when called.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}