using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class AddViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        public AddViewModel()
        {
            transactionOverview = new TransactionOverview();
            items = new List<Item>();
            transactionOverview.Date = DateTime.Now;

            int iHighestTransactionNumber = 0;
            try
            {
                iHighestTransactionNumber = m_Context.TransactionOverviews.Max(t => t.TransactionNumber);
            }
            catch (System.InvalidOperationException e)
            {
                iHighestTransactionNumber = 0;
            }

            transactionOverview.TransactionNumber = iHighestTransactionNumber + 1;
            return;
        }

        public AddViewModel(Item _item, AddViewModel prevVM)
        {
            items = prevVM.items;
            items.Add(_item);
            transactionOverview = prevVM.transactionOverview;
            transactionOverview.TotalValue += _item.ItemPrice;
        }

        public AddViewModel(Customer _customer, AddViewModel prevVM)
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
                return m_Context.Customers.Where(C => C.CustomerDeleted == false).ToList();
            }
        }

        public List<Item> ItemList
        {
            get
            {
                var preFix = m_Context.Items.ToList();
                foreach(Item i in items)
                {
                    foreach(Item it in preFix)
                    {
                        if (i.ItemId == it.ItemId)
                        {
                            it.Stock -= 1;
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
    }
}