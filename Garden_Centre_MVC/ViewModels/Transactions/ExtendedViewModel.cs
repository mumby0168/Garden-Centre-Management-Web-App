using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class ExtendedViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        private int m_TransactionNumber = 0;
        public List<Transaction> transactions
        {
            get
            {
                List<Transaction> ret = m_Context.Transactions.Where(t => t.TransactionNumber == m_TransactionNumber).ToList();
                foreach(Transaction t in ret)
                {
                    if(t.Item == null)
                    {
                        t.Item = m_Context.Items.Where(i => i.ItemId == t.ItemId).First();
                    }
                }
                return ret;
            }
        }

        public ExtendedViewModel(int transactionNumber)
        {
            m_TransactionNumber = transactionNumber;
        }
    }
}