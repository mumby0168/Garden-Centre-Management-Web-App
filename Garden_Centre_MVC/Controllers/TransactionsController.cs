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

        public PartialViewResult HistoricView()
        {
            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }

        public PartialViewResult AddView()
        {
            AddViewModel vm = new AddViewModel();
            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddCustomer(string svm, int id)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(svm);

            foreach (Customer cust in vm.CustomerList)
            {
                if (cust.CustomerId == id)
                    vm._Customer = cust;
            }

            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddItem(string svm, int id)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(svm);

            foreach (Item item in vm.ItemList)
            {
                if (item.ItemId == id)
                    vm._Item = item;
            }

            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddTransaction(string svm)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(svm);
            vm.SaveTransaction();

            IndexViewModel retVm = new IndexViewModel();
            return PartialView("Partials/HistoricView", retVm);
        }
    }
}