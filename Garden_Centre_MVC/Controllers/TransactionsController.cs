using Garden_Centre_MVC.Attributes;
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
        protected override void Dispose(bool disposing)
        {
            m_Context.Dispose();
            base.Dispose(disposing);
        }

        [NormalUser]
        public ActionResult Index()
        {
            return View();
        }

        /////////////////
        //HISTORIC VIEW//
        /////////////////
        [NormalUser]
        public PartialViewResult HistoricView()
        {
            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }

        /////////////////
        //EXTENDED VIEW//
        /////////////////
        [NormalUser]
        public PartialViewResult ExtendedView(int _transactionNumber)
        {
            ExtendedViewModel vm = new ExtendedViewModel(_transactionNumber);
            return PartialView("Partials/ExtendedView", vm);
        }

        //////////////////////
        //DELETE TRANSACTION//
        //////////////////////
        [AdminUser]
        public PartialViewResult DeleteTransaction(int _transactionNumber)
        {
            HistoricViewModel vm = new HistoricViewModel();

            m_Context.TransactionOverviews.Remove(m_Context.TransactionOverviews.Where(to => to.TransactionNumber == _transactionNumber).First());
            foreach (Transaction t in m_Context.Transactions.Where(t => t.TransactionNumber == _transactionNumber))
            {
                m_Context.Transactions.Remove(t);

                var item = m_Context.Items.Where(i => i.ItemId == t.ItemId).First();
                item.Stock++;
                item.Sold--;
            }

            m_Context.SaveChanges();

            return PartialView("Partials/HistoricView", vm);
        }

        ////////////
        //EDIT VIEW//
        ////////////
        [AdminUser]
        public PartialViewResult EditView(int _transactionNumber)
        {
            EditViewModel vm = new EditViewModel();
            vm._transactionOverview = m_Context.TransactionOverviews.Where(to => to.TransactionNumber == _transactionNumber).First();
            vm._transactionOverview.Customer = m_Context.Customers.Where(c => c.CustomerId == vm._transactionOverview.CustomerId).First();
            List<Item> items = new List<Item>();

            foreach (Transaction t in m_Context.Transactions.Where(n => n.TransactionNumber == _transactionNumber).ToList())
            {
                items.Add(m_Context.Items.Where(i => i.ItemId == t.ItemId).First());
            }

            vm._items = items;

            return PartialView("Partials/EditView", vm);
        }

        [AdminUser]
        public PartialViewResult EditSelectCustomer(int customerId, string prevVM)
        {
            EditViewModel vm = new EditViewModel(m_Context.Customers.Where(c => c.CustomerId == customerId).First(), JsonConvert.DeserializeObject<EditViewModel>(prevVM));
            return PartialView("Partials/EditView", vm);
        }

        [AdminUser]
        public PartialViewResult EditAddItem(int itemId, string prevVM)
        {
            EditViewModel vm = new EditViewModel(m_Context.Items.Where(i => i.ItemId == itemId).First(), JsonConvert.DeserializeObject<EditViewModel>(prevVM));
            return PartialView("Partials/EditView", vm);
        }

        [AdminUser]
        public PartialViewResult EditRemItem(int index, string prevVM)
        {
            EditViewModel vm = JsonConvert.DeserializeObject<EditViewModel>(prevVM);

            if (index > vm._items.Count)
            {
                index -= vm._items.Count;
                vm._transactionOverview.TotalValue -= vm._newItems[index].ItemPrice;
                vm._newItems.RemoveAt(index);
            }
            else
            {
                vm._transactionOverview.TotalValue -= vm._items[index].ItemPrice;
                vm._remItemsIndex.Add(index);
                vm._remItemsIds.Add(vm._items[index].ItemId);
                vm._items.RemoveAt(index);
            }

            vm.HasChanged = true;

            return PartialView("Partials/EditView", vm);
        }

        [AdminUser]
        public PartialViewResult SerializeEdit(string prevVM)
        {
            EditViewModel editVM = JsonConvert.DeserializeObject<EditViewModel>(prevVM);

            TransactionOverview to = new TransactionOverview();
            to.CustomerId = editVM._transactionOverview.CustomerId;
            to.Date = editVM._transactionOverview.Date;
            to.TransactionNumber = editVM._transactionOverview.TransactionNumber;
            to.TotalValue = editVM._transactionOverview.TotalValue;

            var entry = m_Context.TransactionOverviews.Where(t => t.TransactionNumber == editVM._transactionOverview.TransactionNumber).First();
            entry.TransactionNumber = to.TransactionNumber;
            entry.TotalValue = to.TotalValue;
            entry.CustomerId = to.CustomerId;
            entry.Date = to.Date;

            if (editVM._remItemsIndex.Count > 0)
            {
                List<Transaction> remList = new List<Transaction>();
                var ts = m_Context.Transactions.Where(t => t.TransactionNumber == to.TransactionNumber).ToList();
                foreach (int index in editVM._remItemsIndex)
                {
                    if(index < ts.Count)
                        remList.Add(ts[index]);
                }

                foreach (Transaction rem in remList)
                {
                    m_Context.Transactions.Remove(rem);

                    var item = m_Context.Items.Where(i => i.ItemId == rem.ItemId).First();
                    item.Stock++;
                    item.Sold--;
                }
            }

            foreach (Item i in editVM._newItems)
            {
                Transaction t = new Transaction();
                t.ItemId = i.ItemId;
                t.TransactionNumber = editVM._transactionOverview.TransactionNumber;
                t.Date = editVM._transactionOverview.Date;
                t.CustomerId = editVM._transactionOverview.CustomerId;

                m_Context.Transactions.Add(t);

                var item = m_Context.Items.Where(it => it.ItemId == t.ItemId).First();
                item.Stock--;
                item.Sold++;
            }

            m_Context.SaveChanges();

            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }

        ////////////
        //ADD VIEW//
        ////////////
        [NormalUser]
        public PartialViewResult AddView()
        {
            AddViewModel vm = new AddViewModel();
            return PartialView("Partials/AddView", vm);
        }

        [NormalUser]
        public PartialViewResult AddItem(int itemId, string prevVM)
        {
            AddViewModel vm = new AddViewModel(m_Context.Items.Where(i => i.ItemId == itemId).First(), JsonConvert.DeserializeObject<AddViewModel>(prevVM));
            return PartialView("Partials/AddView", vm);
        }

        [NormalUser]
        public PartialViewResult SelectCustomer(int customerId, string prevVM)
        {
            AddViewModel vm = new AddViewModel(m_Context.Customers.Where(c => c.CustomerId == customerId).First(), JsonConvert.DeserializeObject<AddViewModel>(prevVM));
            return PartialView("Partials/AddView", vm);
        }

        [NormalUser]
        public PartialViewResult AddRemItem(int index, string prevVM)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(prevVM);

            vm.transactionOverview.TotalValue -= vm.items[index].ItemPrice;
            vm.items.RemoveAt(index);

            return PartialView("Partials/AddView", vm);
        }

        [NormalUser]
        public PartialViewResult SerializeAdd(string prevVm)
        {
            AddViewModel addVM = JsonConvert.DeserializeObject<AddViewModel>(prevVm);

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

                var item = m_Context.Items.Where(it => it.ItemId == t.ItemId).First();
                item.Stock--;
                item.Sold++;
            }

            m_Context.SaveChanges();

            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }
    }
}