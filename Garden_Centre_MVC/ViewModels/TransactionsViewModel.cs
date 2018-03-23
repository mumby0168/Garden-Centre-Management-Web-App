using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels
{
    public class TransactionsViewModel
    {
        public TransactionsViewModel(List<Models.Transaction> transactions)
        {
            Transactions = transactions;
            return;
        }

        private TransactionsViewModel()
        {
            return;
        }

       public List<Models.Transaction> Transactions { get; set; }
    }
}