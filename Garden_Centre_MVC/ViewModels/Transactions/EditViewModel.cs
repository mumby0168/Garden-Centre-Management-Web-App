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
                return m_Context.Customers.ToList();
            }
        }

        public List<Item> ItemList
        {
            get
            {
                return m_Context.Items.ToList();
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

        public EditViewModel()
        {
            _newItems = new List<Item>();
            HasChanged = false;
            return;
        }

        public EditViewModel(Item item, EditViewModel vm)
        {
            _newItems = vm._newItems;
            _items = vm._items;
            _transactionOverview = vm._transactionOverview;
            _items.Add(item);
            _newItems.Add(item);
            _transactionOverview.TotalValue += item.ItemPrice;

            HasChanged = true;
        }

        public EditViewModel(Customer customer, EditViewModel vm)
        {
            _newItems = vm._newItems;
            _items = vm._items;
            _transactionOverview = vm._transactionOverview;
            _transactionOverview.Customer = customer;
            _transactionOverview.CustomerId = customer.CustomerId;

            HasChanged = true;
        }
    }
}