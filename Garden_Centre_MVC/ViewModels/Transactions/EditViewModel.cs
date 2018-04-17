using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class EditViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        public EditViewModel()
        {
            return;
        }

        public EditViewModel(TransactionOverview to, List<Item> _items)
        {
            transactionOverview = to;
            items = _items;

            return;
        }

        public EditViewModel(Item _item, EditViewModel prevVM)
        {
            items = prevVM.items;
            items.Add(_item);
            transactionOverview = prevVM.transactionOverview;
            transactionOverview.TotalValue += _item.ItemPrice;
        }

        public EditViewModel(Customer _customer, EditViewModel prevVM)
        {
            items = prevVM.items;
            transactionOverview = prevVM.transactionOverview;
            transactionOverview.Customer = _customer;
            transactionOverview.CustomerId = _customer.CustomerId;
        }

        public List<Item> items
        {
            get; set;
        }

        public TransactionOverview transactionOverview
        {
            get; set;
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
    }
}