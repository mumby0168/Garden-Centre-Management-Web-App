using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

        /////////////////
        //HISTORIC VIEW//
        /////////////////
        public PartialViewResult HistoricView()
        {
            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }

        /////////////////
        //EXTENDED VIEW//
        /////////////////
        public PartialViewResult ExtendedView(int _transactionNumber)
        {
            ExtendedViewModel vm = new ExtendedViewModel(_transactionNumber);
            return PartialView("Partials/ExtendedView", vm);
        }

        //////////////////////
        //DELETE TRANSACTION//
        //////////////////////
        public PartialViewResult DeleteTransaction(int _transactionNumber)
        {
            HistoricViewModel vm = new HistoricViewModel();

            m_Context.TransactionOverviews.Remove(m_Context.TransactionOverviews.Where(to => to.TransactionNumber == _transactionNumber).First());
            foreach (Transaction t in m_Context.Transactions.Where(t => t.TransactionNumber == _transactionNumber))
            {
                m_Context.Transactions.Remove(t);
            }

            m_Context.SaveChanges();

            return PartialView("Partials/HistoricView", vm);
        }

        ////////////
        //ADD VIEW//
        ////////////
        public PartialViewResult AddView()
        {
            AddViewModel vm = new AddViewModel();
            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult AddItem(int itemId, string prevVM)
        {
            AddViewModel vm = new AddViewModel(m_Context.Items.Where(i => i.ItemId == itemId).First(), JsonConvert.DeserializeObject<AddViewModel>(prevVM));
            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult SelectCustomer(int customerId, string prevVM)
        {
            AddViewModel vm = new AddViewModel(m_Context.Customers.Where(c => c.CustomerId == customerId).First(), JsonConvert.DeserializeObject<AddViewModel>(prevVM));
            return PartialView("Partials/AddView", vm);
        }

        public PartialViewResult SerializeAdd(string prevVM)
        {
            AddViewModel addVM = JsonConvert.DeserializeObject<AddViewModel>(prevVM);

            TransactionOverview to = new TransactionOverview();
            to.CustomerId = addVM.transactionOverview.CustomerId;
            to.Date = addVM.transactionOverview.Date;
            to.TransactionNumber = addVM.transactionOverview.TransactionNumber;
            to.TotalValue = addVM.transactionOverview.TotalValue;

            m_Context.TransactionOverviews.Add(to);

            foreach (Item i in addVM.items)
            {
                Transaction t = new Transaction();
                t.ItemId = i.ItemId;
                t.TransactionNumber = addVM.transactionOverview.TransactionNumber;
                t.Date = addVM.transactionOverview.Date;
                t.CustomerId = addVM.transactionOverview.CustomerId;
                m_Context.Transactions.Add(t);
            }

            m_Context.SaveChanges();

            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }
    }
}