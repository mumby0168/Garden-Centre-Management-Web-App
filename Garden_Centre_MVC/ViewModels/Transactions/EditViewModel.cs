using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class EditViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        public List<Customer> CustomerList
        {
            get
            {
                return m_Context.Customers.Where(C => C.CustomerDeleted == false).ToList();
            }
        }

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

        public bool HasChanged
        {
            get; set;
        }

        public TransactionOverview _transactionOverview
        {
            get; set;
        }

        public List<Item> _items
        {
            get; set;
        }

        public List<Item> _newItems
        {
            get; set;
        }

        public List<int> _remItemsIndex
        {
            get; set;
        }

        public List<int> _remItemsIds
        {
            get; set;
        }

        public EditViewModel()
        {
            _newItems = new List<Item>();
            _remItemsIndex = new List<int>();
            _remItemsIds = new List<int>();
            HasChanged = false;
            return;
        }

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