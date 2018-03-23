using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class IndexViewModel
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public List<TransactionOverview> Transactions
        {
            get
            {
                return m_Context.GroupedTransactions;
            }
        }
    }
}