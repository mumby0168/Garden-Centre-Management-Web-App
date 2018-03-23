using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Models
{
    public class TransactionOverview
    {
        private List<Transaction> m_Transactions = null;
        public List<Transaction> Transactions
        {
            get
            {
                return m_Transactions;
            }
        }

        private DateTime m_DateAndTime;
        public DateTime DateAndTime
        {
            get
            {
                return m_DateAndTime;
            }
        }

        private int m_Id = 0;
        public int ID
        {
            get
            {
                return m_Id;
            }
        }

        private float m_Value = 0.0f;
        public float TotalValue
        {
            get
            {
                return m_Value;
            }
        }

        public TransactionOverview(List<Transaction> transactions)
        {
            m_Transactions = transactions;
            m_Value = 0.0f;
            m_Id = transactions[0].TransactionNumber;
            m_DateAndTime = transactions[0].Date;
            foreach (Transaction t in m_Transactions)
            {
                m_Value += t.Item.ItemPrice;
            }
        }
    }
}