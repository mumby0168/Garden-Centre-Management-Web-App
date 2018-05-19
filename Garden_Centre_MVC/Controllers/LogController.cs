using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.LogViewModels;
using Newtonsoft.Json;

namespace Garden_Centre_MVC.Controllers
{
    public class LogController : Controller
    {
        private DatabaseContext _context;

        /// <summary>
        /// this will be called when the controller method is made.
        /// it will create a instance of the database contect 
        /// </summary>
        public LogController()
        {
            _context = new DatabaseContext();
        }

        /// <summary>
        /// this is the default method for the controller.
        /// it will return a list of logs the most recent action first.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var logs =_context.Logs
                .Include(l => l.ActionType)
                .Include(e => e.EmployeeLogin.Employee)
                .OrderByDescending(l => l.DateOfAction)
                .ToList();

            var vm = new LogLandingViewModel()
            {
                Logs = logs
            };

            return View("LogLandingPage", vm);
        }

        /// <summary>
        /// this method again will return all of the logs that are in the database
        /// </summary>
        /// <returns></returns>
        public string GetAll()
        {
            var logs = _context.Logs.Include(l => l.ActionType).Include(e => e.EmployeeLogin.Employee).OrderByDescending(l => l.DateOfAction).ToList();


            List<object> returns = new List<object>();


            foreach (var log in logs)
            {
                returns.Add(new
                {
                    logNumber = log.LogId, DateofAction = log.DateOfAction.ToString("R"), Username = log.EmployeeLogin.Username, ActionType = log.ActionType.Description, DetailsOfAction = log.PropertyEffected});
            }

            return JsonConvert.SerializeObject(returns);
        }

        /// <summary>
        /// this will dispose of any overhanging resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}