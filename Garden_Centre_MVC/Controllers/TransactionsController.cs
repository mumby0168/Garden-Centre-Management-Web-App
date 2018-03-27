using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.Transactions;
using Newtonsoft.Json;

namespace Garden_Centre_MVC.Controllers
{
    public class TransactionsController : Controller, IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }
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

        public PartialViewResult ViewDetails(string svm, int transactionNumber)
        {
            HistoricViewModel vm = JsonConvert.DeserializeObject<HistoricViewModel>(svm);

            ExtendedViewModel retVm = new ExtendedViewModel(vm.GroupTransactions(), transactionNumber);
            return PartialView("Partials/ExtendedView", retVm);
        }

        public PartialViewResult EditTransaction(string svm, int transactionNumber)
        {
            HistoricViewModel vm = JsonConvert.DeserializeObject<HistoricViewModel>(svm);

            foreach (TransactionOverview to in vm.GroupTransactions())
            {
                if (to.ID == transactionNumber)
                {
                    return PartialView("Partials/EditView", new EditViewModel(to));
                }
            }

            return null;
        }

        public PartialViewResult EditDeleteTransaction(string svm, int transactionId)
        {
            EditViewModel ivm = JsonConvert.DeserializeObject<EditViewModel>(svm);
            EditViewModel vm = new EditViewModel(ivm.TransactionOverview, ivm._Items, ivm._Remove, ivm._Customer);

            vm._Remove.Add(m_Context.Transactions.Where(m => m.Id == transactionId).First());

            foreach(Transaction t in vm.TransactionOverview.Transactions)
            {
                if(t.Id == transactionId)
                {
                    vm.TransactionOverview.Transactions.Remove(t);
                    break;
                }
            }

            return PartialView("Partials/EditView", vm);
        }

        public PartialViewResult EditAddItem(string svm, int itemId)
        {
            EditViewModel ivm = JsonConvert.DeserializeObject<EditViewModel>(svm);
            EditViewModel vm = new EditViewModel(ivm.TransactionOverview, ivm._Remove, ivm._Customer);
            foreach (Item i in m_Context.Items)
            {
                if (i.ItemId == itemId)
                {
                    vm._Items.Add(i);
                    break;
                }
            }

            vm.UpdateVM();

            return PartialView("Partials/EditView", vm);
        }

        public PartialViewResult EditSave(string svm)
        {
            EditViewModel ivm = JsonConvert.DeserializeObject<EditViewModel>(svm);
            EditViewModel ovm = new EditViewModel(ivm.TransactionOverview, ivm._Items, ivm._Remove, ivm._Customer);
            ovm.Save();

            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }

        public PartialViewResult EditCustomer(string svm, int id)
        {
            EditViewModel ivm = JsonConvert.DeserializeObject<EditViewModel>(svm);
            EditViewModel vm = new EditViewModel(ivm.TransactionOverview, ivm._Items, ivm._Remove, m_Context.Customers.Where<Customer>(m => m.CustomerId == id).First());

            return PartialView("Partials/EditView", vm);
        }

        public PartialViewResult DeleteTransaction(int transactionNumber)
        {
            List<Transaction> remove = new List<Transaction>();
            foreach(Transaction t in m_Context.Transactions)
            {
                if (t.TransactionNumber == transactionNumber)
                    remove.Add(t);
            }

            foreach(Transaction t in remove)
            {
                m_Context.Transactions.Remove(t);
            }

            m_Context.SaveChanges();

            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }
    }
}