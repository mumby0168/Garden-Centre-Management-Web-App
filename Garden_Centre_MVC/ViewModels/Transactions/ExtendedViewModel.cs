using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    /// <summary>
    /// This view model represents each part of the transaction that make up a single transaction.
    /// This follows on from the architecture that a transaction overview stores the transaction number for each part (item) of the transaction
    /// </summary>
    public class ExtendedViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        /// <summary>
        /// store the transaction nubmer.
        /// </summary>
        private int m_TransactionNumber = 0;

        /// <summary>
        // Retrive the list of transactions that make up the transaction from the database.
        /// </summary>
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

        /// <summary>
        /// overloaded version of the constructor to set the transaction number
        /// </summary>
        public ExtendedViewModel(int transactionNumber)
        {
            m_TransactionNumber = transactionNumber;
        }
    }
}