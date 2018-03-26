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
            AddViewModel m = JsonConvert.DeserializeObject<AddViewModel>(svm);
            AddViewModel vm = new AddViewModel(m);


            foreach (Customer cust in vm.CustomerList)
            {
                if (cust.CustomerId == id)
                    vm._Customer = cust;
            }

            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddItem(string svm, int id)
        {
            AddViewModel m = JsonConvert.DeserializeObject<AddViewModel>(svm);
            AddViewModel vm = new AddViewModel(m);

            foreach (Item item in vm.ItemList)
            {
                if (item.ItemId == id)
                    vm._Items.Add(item);
            }

            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddTransaction(string svm)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(svm);
            vm.SaveTransaction();

            HistoricViewModel retVm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", retVm);
        }
    }
}