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
            var logs =_context.Logs.Include(l => l.ActionType).Include(e => e.EmployeeLogin.Employee).OrderByDescending(l => l.DateOfAction).ToList();

            var vm = new LogLandingViewModel()
            {
                Logs = logs
            };

            return View("LogLandingPage", vm);
        }

        public ActionResult GetAll()
        {
            var logs = _context.Logs.Include(l => l.ActionType).Include(e => e.EmployeeLogin.Employee).OrderByDescending(l => l.DateOfAction).ToList();


            List<object> returns = new List<object>();


            foreach (var log in logs)
            {
                returns.Add(new
                {
                    logNumber = log.LogId, DateofAction = log.DateOfAction, Username = log.EmployeeLogin.Username, ActionType = log.ActionType, DetailsOfAction = log.PropertyEffected});
            }

            return Json(returns, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}