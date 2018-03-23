using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.ViewModels.Transactions;

namespace Garden_Centre_MVC.Controllers
{
    public class TransactionsController : Controller
    {
        public ActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            return View("Index", vm);
        }
    }
}