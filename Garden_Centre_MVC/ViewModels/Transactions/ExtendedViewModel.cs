using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class ExtendedViewModel
    {
        public ExtendedViewModel(List<TransactionOverview> transactionOverviews, int transactionNum)
        {
            foreach (TransactionOverview to in transactionOverviews)
            {
                if(to.ID == transactionNum)
                {
                    m_TransactionOverview = to;
                    break;
                }
            }
        }

        private ExtendedViewModel()
        {
            return;
        }

        private TransactionOverview m_TransactionOverview = null;
        public TransactionOverview TransactionOverview
        {
            get
            {
                return m_TransactionOverview;
            }
        }
    }
}