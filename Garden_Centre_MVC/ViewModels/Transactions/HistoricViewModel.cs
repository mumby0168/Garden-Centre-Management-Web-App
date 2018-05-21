using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.Entity;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class HistoricViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        public List<TransactionOverview> TransactionOverviews
        {
            get
            {
                List<TransactionOverview> ret = m_Context.TransactionOverviews.Include(t => t.Customer).ToList();
                return ret;
            }
        }

        public string TransactionOverviewsJSON
        {
            get
            {
                
                String s = JsonConvert.SerializeObject(TransactionOverviews);
                return s;
            }
        }
    }
}