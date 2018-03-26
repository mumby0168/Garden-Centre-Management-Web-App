using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.EmployeeViewModels;

namespace Garden_Centre_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private DatabaseContext _context;

        public EmployeeController()
        {
            _context = new DatabaseContext();
        }


        // GET: Employee
        public ActionResult Index()
        {
            var employees = _context.Employees.ToList();

            var vm = new EmployeeLandingViewModels {Employees = employees};

            //returns the home view
            return View("EmployeeLanding", vm);
        }

        public ActionResult Save(int id)
        {
            if (id == 0)
            {
                //then add the customer as they must be new
                return View();
            }
            else
            {
                //if not then we need to edit the customer record [Use Automapper]
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            var vm = new EmployeeFormViewModel()
            {
                Employee = employee
            };

            return PartialView("EmployeeForm", vm);

        }

        /// <summary>
        /// gets all of the employees to be viewed in the table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            return View();
        }

        public ActionResult GetSingle(int id)
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}