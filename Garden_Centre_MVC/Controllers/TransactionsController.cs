using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.ViewModels.Transactions;
using Newtonsoft.Json;

namespace Garden_Centre_MVC.Controllers
{
    public class TransactionsController : Controller
    {
        public ActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            return View("Index", vm);
        }

        public PartialViewResult AddView()
        {
            AddViewModel vm = new AddViewModel();
            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddTransaction(string svm, int id)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(svm);
            vm._Item = id;
            vm.AddItem();
            return PartialView("Partials/AddView", vm);
        }
    }
}