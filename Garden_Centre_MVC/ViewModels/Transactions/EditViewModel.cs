using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class EditViewModel : IDisposable
    {
        public EditViewModel(){}

        public EditViewModel(TransactionOverview to)
        {
            TransactionOverview = to;
            _Customer = to.Transactions[0].Customer;
            _Items = new List<Item>();
            _Remove = new List<Transaction>();

            if (ItemList == null || ItemList.Count == 0)
                ItemList = m_Context.Items.ToList();

            if (CustomerList == null || CustomerList.Count == 0)
                CustomerList = m_Context.Customers.ToList();
        }

        public EditViewModel(TransactionOverview to, List<Transaction> remove, Customer cust)
        {
            TransactionOverview = to;
            _Customer = cust;
            _Items = new List<Item>();
            _Remove = remove;

            if (ItemList == null || ItemList.Count == 0)
                ItemList = m_Context.Items.ToList();

            if (CustomerList == null || CustomerList.Count == 0)
                CustomerList = m_Context.Customers.ToList();
        }

        public EditViewModel(TransactionOverview to, List<Item> items, List<Transaction> remove, Customer customer)
        {
            TransactionOverview = to;
            _Customer = customer;
            _Items = items;
            _Remove = remove;

            if (ItemList == null || ItemList.Count == 0)
                ItemList = m_Context.Items.ToList();

            if (CustomerList == null || CustomerList.Count == 0)
                CustomerList = m_Context.Customers.ToList();
        }

        public void Save()
        {
            if(_Customer != TransactionOverview.Transactions[0].Customer)
            {
                foreach (Transaction t in m_Context.Transactions)
                {
                    if (t.TransactionNumber == TransactionOverview.ID)
                        t.CustomerId = _Customer.CustomerId;
                }
            }

            if(_Items.Count > 0)
            {
                foreach(Item i in _Items)
                {
                    Transaction transaction = new Transaction();
                    transaction.CustomerId = _Customer.CustomerId;
                    transaction.ItemId = i.ItemId;
                    transaction.Date = TransactionOverview.DateAndTime;
                    transaction.TransactionNumber = TransactionOverview.ID;
                    m_Context.Transactions.Add(transaction);
                }
            }

            if (_Remove.Count > 0)
            {
                foreach (Transaction t in _Remove)
                {
                    m_Context.Transactions.Remove(m_Context.Transactions.Where(m => m.Id == t.Id).First());
                }
            }


            m_Context.SaveChanges();
        }

        public void UpdateVM()
        {
            if (_Items.Count > 0)
            {
                foreach (Item i in _Items)
                {
                    Transaction transaction = new Transaction();
                    transaction.Customer = _Customer;
                    transaction.CustomerId = _Customer.CustomerId;
                    transaction.Item = i;
                    transaction.ItemId = i.ItemId;
                    transaction.Date = TransactionOverview.DateAndTime;
                    transaction.TransactionNumber = TransactionOverview.ID;
                    TransactionOverview.Transactions.Add(transaction);
                }
            }
        }

        public Customer _Customer
        {
            get; set;
        }

        public List<Item> _Items
        {
            get; set;
        }
        
        public List<Transaction> _Remove
        {
            get; set;
        }

        public TransactionOverview TransactionOverview
        {
            get; set;
        }

        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        public List<Customer> CustomerList
        {
            private set; get;
        }
        public List<Item> ItemList
        {
            private set; get;
        }
    }
}