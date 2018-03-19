using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garden_Centre_MVC.Controllers
{
    public class TransactionsController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}