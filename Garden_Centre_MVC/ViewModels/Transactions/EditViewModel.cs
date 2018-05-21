using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    /// <summary>
    /// This view model represents the edit form.
    /// It is responsible for providing all the relevant data.
    /// </summary>
    public class EditViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        /// <summary>
        /// Returns a list of all the customers in the database.
        /// </summary>
        public List<Customer> CustomerList
        {
            get
            {
                return m_Context.Customers.Where(C => C.CustomerDeleted == false).ToList();
            }
        }

        /// <summary>
        /// This returns a list of all the available items that can be added, removing any that are out of stock
        /// </summary>
        public List<Item> ItemList
        {
            get
            {
                var preFix = m_Context.Items.ToList();
                foreach(Item i in _newItems)
                {
                    foreach(Item it in preFix)
                    {
                        if (i.ItemId == it.ItemId)
                        {
                            it.Stock -= 1;
                        }
                    }
                }

                foreach (int s in _remItemsIds)
                {
                    var i = m_Context.Items.Where(n => n.ItemId == s).First();
                    foreach (Item it in preFix)
                    {
                        if (i.ItemId == it.ItemId)
                        {
                            it.Stock += 1;
                        }
                    }
                }

                List<Item> postFix = new List<Item>();
                foreach(Item i in preFix)
                {
                    if(i.Stock > 0)
                    {
                        postFix.Add(i);
                    }
                }

                m_Context.Dispose();
                m_Context = new DatabaseContext();

                return postFix;
            }
        }

        /// <summary>
        /// A flag denoting that the view model has changed.
        /// </summary>
        public bool HasChanged
        {
            get; set;
        }

        /// <summary>
        /// Stores the instance of the transaction overview been used.
        /// </summary>
        public TransactionOverview _transactionOverview
        {
            get; set;
        }

        /// <summary>
        /// Stores the list of items that are in the transactions.
        /// </summary>
        public List<Item> _items
        {
            get; set;
        }

        /// <summary>
        /// Stores the list of items that have been added since starting editing.
        /// </summary>
        public List<Item> _newItems
        {
            get; set;
        }

        /// <summary>
        /// Stores the indexs of the items that have been removed.
        /// </summary>
        public List<int> _remItemsIndex
        {
            get; set;
        }

        /// <summary>
        /// Stores the items of the items that have been removed
        /// </summary>
        public List<int> _remItemsIds
        {
            get; set;
        }

        /// <summary>
        /// Constructor to create the empty view model.
        /// </summary>
        public EditViewModel()
        {
            _newItems = new List<Item>();
            _remItemsIndex = new List<int>();
            _remItemsIds = new List<int>();
            HasChanged = false;
            return;
        }

        /// <summary>
        /// Overloaded constructor takes the item to add and the previous view model to populate the new one.
        /// </summary>
        public EditViewModel(Item item, EditViewModel vm)
        {
            _newItems = vm._newItems;
            _remItemsIndex = vm._remItemsIndex;
            _remItemsIds = vm._remItemsIds;
            _items = vm._items;
            _transactionOverview = vm._transactionOverview;
            _newItems.Add(item);
            _transactionOverview.TotalValue += item.ItemPrice;

            HasChanged = true;
        }

        /// <summary>
        /// Overloaded constructor takes the customer to change the selection.
        /// </summary>
        public EditViewModel(Customer customer, EditViewModel vm)
        {
            _newItems = vm._newItems;
            _remItemsIndex = vm._remItemsIndex;
            _remItemsIds = vm._remItemsIds;
            _items = vm._items;
            _transactionOverview = vm._transactionOverview;
            _transactionOverview.Customer = customer;
            _transactionOverview.CustomerId = customer.CustomerId;

            HasChanged = true;
        }
    }
}