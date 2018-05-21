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
    /// <summary>
    ///This view model is used to represent the history of all transactions that have taken place
    /// </summary>
    public class HistoricViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        /// <summary>
        ///This returns a list of transaction overview which represent the transactions that have taken place.
        /// </summary>
        public List<TransactionOverview> TransactionOverviews
        {
            get
            {
                List<TransactionOverview> ret = m_Context.TransactionOverviews.Include(t => t.Customer).ToList();
                return ret;
            }
        }
        //Returns the list of transaction overview as a JSON object.
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