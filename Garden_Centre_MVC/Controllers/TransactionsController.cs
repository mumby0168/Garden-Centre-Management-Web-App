using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garden_Centre_MVC.Controllers
{
    public class TransactionsController : Controller, IDisposable
    {
        private Persistance.DatabaseContext _context;

        void IDisposable.Dispose()
        {
            _context.Dispose();
        }

        public TransactionsController()
        {
            _context = new Persistance.DatabaseContext();
        }

        public ActionResult Index()
        {
            ViewModels.TransactionsViewModel vm = new ViewModels.TransactionsViewModel(_context.Transactions.ToList());
            return View("Index", vm);
        }

        public PartialViewResult AddView()
        {

        }
    }
}