using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using System.Data.Entity;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class HistoricViewModel : IDisposable
    {
        public HistoricViewModel()
        {
            Transactions = GroupTransactions();
        }

        public List<TransactionOverview> GroupTransactions()
        {
            List<List<Transaction>> ret = new List<List<Transaction>>();

            var transactions = m_Context.Transactions.Include(m => m.Item).Include(s => s.Customer).ToList();
            //for every transaction
            for (int x = 0; x < transactions.Count; x++)
            {
                //if the return is empty then add it anyways
                if (ret.Count == 0)
                {
                    ret.Add(new List<Transaction>());
                    ret[0].Add(transactions[x]);
                    continue;
                }

                bool bCreateNew = true;

                //for all groups that are in return
                for (int y = 0; y < ret.Count; y++)
                {
                    if (ret[y][0].TransactionNumber == transactions[x].TransactionNumber)
                    {
                        ret[y].Add(transactions[x]);
                        bCreateNew = false;
                        break;
                    }
                }

                if (bCreateNew)
                {
                    ret.Add(new List<Transaction>());
                    ret[ret.Count - 1].Add(transactions[x]);
                    continue;
                }
            }

            List<TransactionOverview> retA = new List<TransactionOverview>();
            foreach (var collection in ret)
            {
                retA.Add(new TransactionOverview(collection));
            }

            return retA;
        }

        public List<TransactionOverview> Transactions
        {
            get; set;
        }

        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }
    }
}