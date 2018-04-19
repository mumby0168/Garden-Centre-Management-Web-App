using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.LogViewModels;

namespace Garden_Centre_MVC.Controllers
{
    public class LogController : Controller
    {
        private DatabaseContext _context;

        public LogController()
        {
            _context = new DatabaseContext();
        }

        public ActionResult Index()
        {
            var logs =_context.Logs.Include(l => l.ActionType).Include(e => e.EmployeeLogin.Employee).ToList();

            var vm = new LogLandingViewModel()
            {
                Logs = logs
            };

            return View("LogLandingPage", vm);
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}