using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    /// <summary>
    /// This view model is not used
    /// </summary>
    public class IndexViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        public IndexViewModel()
        {
            return;
        }
    }
}