using Garden_Centre_MVC.Assets;
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
using System.Data.Entity;

namespace Garden_Centre_MVC.Controllers
{
    /// <summary>
    /// Robert Bennett -
    /// This class will control anything to do with the transactions from adding, editing, deleting and viewing them.
    /// </summary>
    public class TransactionsController : Controller, IDisposable
    {
        //Stores an instance of the database handler
        private DatabaseContext m_Context = new DatabaseContext();
        //Derrived from IDisposable - called when the object is garbage collected
        protected override void Dispose(bool disposing)
        {
            //disposes the database handle
            m_Context.Dispose();
            //calls the base method
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called to get the index page - not used as we use the HistoricView as a partial view
        /// </summary>
        [NormalUser]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This returns the view that lists all of the transactions.
        /// </summary>
        [NormalUser]
        public PartialViewResult HistoricView()
        {
            //Create the view model
            HistoricViewModel vm = new HistoricViewModel();
            //Return the view
            return PartialView("Partials/HistoricView", vm);
        }

        /// <summary>
        /// This returns the details of specific transaction.
        /// <param name="_transactionNumber">This is the number of the transaction to display the extended view for.</param>
        /// </summary>
        [NormalUser]
        public PartialViewResult ExtendedView(int _transactionNumber)
        {
            //create the view model
            ExtendedViewModel vm = new ExtendedViewModel(_transactionNumber);
            //Return the view
            return PartialView("Partials/ExtendedView", vm);
        }

        /// <summary>
        /// This deletes a transaction then returns the historic view again.
        /// <param name="_transactionNumber">This is the number of the transaction to be deleted.</param>
        /// </summary>
        [AdminUser]
        public PartialViewResult DeleteTransaction(int _transactionNumber)
        {
            //Create the view model.
            HistoricViewModel vm = new HistoricViewModel();

            //Remove the transaction overview from the database
            m_Context.TransactionOverviews.Remove(m_Context.TransactionOverviews.Where(to => to.TransactionNumber == _transactionNumber).First());
            //for every transaction in the transaction overview remove it and increase the number in stock and decrease the nunber sold for that item.
            foreach (Transaction t in m_Context.Transactions.Where(t => t.TransactionNumber == _transactionNumber))
            {
                m_Context.Transactions.Remove(t);

                var item = m_Context.Items.Where(i => i.ItemId == t.ItemId).First();
                item.Stock++;
                item.Sold--;
            }

            //Save the database changes
            m_Context.SaveChanges();

            //Log the transaction has been deleted
            Logger.LogAction("Transaction Deleted", "Deleted transaction number " + _transactionNumber);

            //return the view
            return PartialView("Partials/HistoricView", vm);
        }

        /// <summary>
        /// Called when the edit button is clicked for a transaction.
        /// This displays the edit form.
        /// <param name="_transactionNumber">This is the number of the transaction to be edited.</param>
        /// </summary>
        [AdminUser]
        public PartialViewResult EditView(int _transactionNumber)
        {
            //Create the view model
            EditViewModel vm = new EditViewModel();

            //retrive the tranasction overview for the passed in transaction number from the database.
            vm._transactionOverview = m_Context.TransactionOverviews.Include(to => to.Customer).Where(to => to.TransactionNumber == _transactionNumber).First();

            List<Item> items = new List<Item>();
            
            //retrive all the items in that transaction from the database
            foreach (Transaction t in m_Context.Transactions.Where(n => n.TransactionNumber == _transactionNumber).ToList())
            {
                items.Add(m_Context.Items.Where(i => i.ItemId == t.ItemId).First());
            }

            //store the current items in the transaction in the view model
            vm._items = items;

            //return the view
            return PartialView("Partials/EditView", vm);
        }

        /// <summary>
        /// This controller method is called when the user changes the customer selected in the edit view.
        /// <param name="customerId">This is the id of the customer selected.</param>
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [AdminUser]
        public PartialViewResult EditSelectCustomer(int customerId, string prevVM)
        {
            //create the view model and return the view, populate the view model 
            EditViewModel vm = new EditViewModel(m_Context.Customers.Where(c => c.CustomerId == customerId).First(), JsonConvert.DeserializeObject<EditViewModel>(prevVM));
            return PartialView("Partials/EditView", vm);
        }

        /// <summary>
        /// This controller method is called when the user adds an item while in the edit view.
        /// <param name="itemId">This is the id of the item selected to add.</param>
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [AdminUser]
        public PartialViewResult EditAddItem(int itemId, string prevVM)
        {
            //create the view model and return the view, populate the view model 
            EditViewModel vm = new EditViewModel(m_Context.Items.Where(i => i.ItemId == itemId).First(), JsonConvert.DeserializeObject<EditViewModel>(prevVM));
            return PartialView("Partials/EditView", vm);
        }

        /// <summary>
        /// This controller method is called when the user removes an item while in the edit view.
        /// <param name="index">This is the index of the item stored in the items list in the view model.</param>
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [AdminUser]
        public PartialViewResult EditRemItem(int index, string prevVM)
        {
            //create the view model and populate with the previous view model
            EditViewModel vm = JsonConvert.DeserializeObject<EditViewModel>(prevVM);

            //update in stock and sold
            if (index >= vm._items.Count)
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

            //set the has view model changed flag to true
            vm.HasChanged = true;
            //return the view
            return PartialView("Partials/EditView", vm);
        }

        /// <summary>
        /// This controller method is responsable for saving the changes to the transaction to the database.
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [AdminUser]
        public PartialViewResult SerializeEdit(string prevVM)
        {
            //create the view model and pass in the previous view model
            EditViewModel editVM = JsonConvert.DeserializeObject<EditViewModel>(prevVM);

            //create a new transaction over view and populate the variables to store in the data base
            TransactionOverview to = new TransactionOverview();
            to.CustomerId = editVM._transactionOverview.CustomerId;
            to.Date = m_Context.TransactionOverviews.Where(i => i.TransactionNumber == editVM._transactionOverview.TransactionNumber).First().Date;
            to.TransactionNumber = editVM._transactionOverview.TransactionNumber;
            to.TotalValue = editVM._transactionOverview.TotalValue;

            //retrive the current entry for that transaction id from the database
            //then update the entry with the values populated from the the view model
            var entry = m_Context.TransactionOverviews.Where(t => t.TransactionNumber == editVM._transactionOverview.TransactionNumber).First();
            entry.TransactionNumber = to.TransactionNumber;
            entry.TotalValue = to.TotalValue;
            entry.CustomerId = to.CustomerId;
            entry.Date = to.Date;

            //if there are removed items then remove them from the transactions in the database and update the stock and sold variables
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

            //for every new item in the updated transaction the data base with the item
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

            //save the changes
            m_Context.SaveChanges();

            //Log that a transaction has been edited
            Logger.LogAction("Transaction Edited", "Edited transaction number " + to.TransactionNumber);

            //Create the historical view model and return a view.
            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }

        /// <summary>
        /// Called to get the Add View.
        /// </summary>
        [NormalUser]
        public PartialViewResult AddView()
        {
            AddViewModel vm = new AddViewModel();
            return PartialView("Partials/AddView", vm);
        }

        /// <summary>
        /// Called when the add item is pressed in the add view.
        /// <param name="itemId">The id of the item added.</param>
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [NormalUser]
        public PartialViewResult AddItem(int itemId, string prevVM)
        {
            AddViewModel vm = new AddViewModel(m_Context.Items.Where(i => i.ItemId == itemId).First(), JsonConvert.DeserializeObject<AddViewModel>(prevVM));
            return PartialView("Partials/AddView", vm);
        }

        /// <summary>
        /// Called when a new customer is selected in the add view.
        /// <param name="customerId">The id of the customer added.</param>
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [NormalUser]
        public PartialViewResult SelectCustomer(int customerId, string prevVM)
        {
            AddViewModel vm = new AddViewModel(m_Context.Customers.Where(c => c.CustomerId == customerId).First(), JsonConvert.DeserializeObject<AddViewModel>(prevVM));
            return PartialView("Partials/AddView", vm);
        }

        /// <summary>
        /// Called when a new customer is selected in the add view.
        /// <param name="index">The index of the item to be removed from the item list.</param>
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [NormalUser]
        public PartialViewResult AddRemItem(int index, string prevVM)
        {
            AddViewModel vm = JsonConvert.DeserializeObject<AddViewModel>(prevVM);

            vm.transactionOverview.TotalValue -= vm.items[index].ItemPrice;
            vm.items.RemoveAt(index);

            return PartialView("Partials/AddView", vm);
        }

        /// <summary>
        /// Called when the save button is clicked in the add view.
        /// <param name="prevVM">The previous vm this is passed through AJAX and is the one that was been used.</param>
        /// </summary>
        [NormalUser]
        public PartialViewResult SerializeAdd(string prevVm)
        {
            //create view model from the vm passed from the view
            AddViewModel addVM = JsonConvert.DeserializeObject<AddViewModel>(prevVm);

            //Create a transaction overview to summerise the transaction been added
            TransactionOverview to = new TransactionOverview();
            to.CustomerId = addVM.transactionOverview.CustomerId;
            to.Date = addVM.transactionOverview.Date;
            to.TransactionNumber = addVM.transactionOverview.TransactionNumber;
            to.TotalValue = addVM.transactionOverview.TotalValue;

            //Add the transaction overview
            m_Context.TransactionOverviews.Add(to);

            //for every item added create a transaction and update the stock and sold
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

            //Save the changes to the database
            m_Context.SaveChanges();

            //log that a transaction has been added
            Logger.LogAction("Transaction Added", "Added transaction number " + to.TransactionNumber);

            //create a new vm and return the view
            HistoricViewModel vm = new HistoricViewModel();
            return PartialView("Partials/HistoricView", vm);
        }
    }
}