using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Models
{
    public class TransactionOverview
    {
        public List<Transaction> Transactions
        {
            get; set;
        }

        public DateTime DateAndTime
        {
            get; set;
        }

        public int ID
        {
            set; get;
        }
       
        public float TotalValue
        {
            set; get;
        }

        public TransactionOverview()
        {
            Transactions = new List<Transaction>();
            TotalValue = 0.0f;
            DateAndTime = DateTime.Now;
            foreach (Transaction t in Transactions)
            {
                TotalValue += t.Item.ItemPrice;
            }
        }

        public TransactionOverview(List<Transaction> transactions)
        {
            Transactions = transactions;
            TotalValue = 0.0f;
            ID = transactions[0].TransactionNumber;
            DateAndTime = transactions[0].Date;
            foreach (Transaction t in Transactions)
            {
                TotalValue += t.Item.ItemPrice;
            }
        }
    }
}