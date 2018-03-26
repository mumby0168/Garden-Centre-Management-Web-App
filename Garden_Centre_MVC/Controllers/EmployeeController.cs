using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Models;
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

        [HttpPost]
        public ActionResult Save(EmployeeFormViewModel emp)
        {
            if (emp.Employee.EmployeeId == 0)
            {
                var employee = new Employee();
                employee.Admin = emp.Employee.Admin;
                employee.EmployeeNumber = emp.Employee.EmployeeNumber;
                employee.FirstName = emp.Employee.FirstName;
                employee.SecondName = emp.Employee.SecondName;

                _context.Employees.Add(employee);
                _context.SaveChanges();

                var vm = new EmployeeLandingViewModels()
                {
                    Employees = _context.Employees.ToList()
                };

                return View("EmployeeLanding", vm);
            }
            else
            {
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == emp.Employee.EmployeeId);

                employee.Admin = emp.Employee.Admin;
                employee.EmployeeNumber = emp.Employee.EmployeeNumber;
                employee.FirstName = emp.Employee.FirstName;
                employee.SecondName = emp.Employee.SecondName;
               
                _context.SaveChanges();


                var vm = new EmployeeLandingViewModels()
                {
                    Employees = _context.Employees.ToList()
                };

                return PartialView("EmployeeLanding", vm);
            }
        }

        public ActionResult Remove(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            var vm = new EmployeeLandingViewModels()
            {
                Employees = _context.Employees.ToList()
            };

            return PartialView("EmployeeLanding", vm);
        }

        public ActionResult Add()
        {
            var vm = new EmployeeFormViewModel();

            return PartialView("EmployeeForm", vm);
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