using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class AddViewModel : IDisposable
    {
        public AddViewModel()
        {
            if(ItemList == null || ItemList.Count == 0)
                ItemList = m_Context.Items.ToList();

            if (CustomerList == null || CustomerList.Count == 0)
                CustomerList = m_Context.Customers.ToList();

            m_Items = new List<Item>();
        }

        public AddViewModel(AddViewModel vm)
        {
            throw new Exception("....");
        }

        public Customer _Customer
        {
            get; set;
        }

        public void SaveTransaction()
        {
            foreach (Item item in this._Items)
            {
                Transaction transaction = new Transaction();
                transaction.CustomerId = _Customer.CustomerId;
                transaction.ItemId = item.ItemId;
                transaction.Date = DateTime.Now;
                transaction.TransactionNumber = TransactionId;
                m_Context.Transactions.Add(transaction);
            }

            m_Context.SaveChanges();
        }

        public Item _Item
        {
            get; set;
        }

        public int TransactionId
        {
            get
            {
                int iHighest = 1;
                foreach (Transaction t in m_Context.Transactions)
                {
                    if (t.TransactionNumber > iHighest)
                        iHighest = t.TransactionNumber;
                }

                return iHighest + 1;
            }
        }

        private List<Item> m_Items = null;
        public List<Item> _Items
        {
            get
            {
                return m_Items;
            }
        }

        private DatabaseContext m_Context = new DatabaseContext();

        public List<Customer> CustomerList
        {
            private set;  get;
        }

        public List<Item> ItemList
        {
            private set; get;
        }

        public void Dispose()
        {
            m_Context.Dispose();
        }
    }
}