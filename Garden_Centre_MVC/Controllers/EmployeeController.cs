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
            var employees = _context.Employees.Take(10).ToList();

            var vm = new EmployeeLandingViewModels {Employees = employees, PageNum = 1};

            //returns the home view
            return PartialView("EmployeeLanding", vm);
        }

        public ActionResult CheckAmountOfRecords()
        {
            var count = _context.Employees.Count();

            return Json(new {amount=count.ToString()});
        }

        public ActionResult LoadTablePage(int page)
        {
            var skipAmount = (page -1) * 10;
            var employees = _context.Employees.OrderBy(e => e.EmployeeId).Skip(skipAmount).Take(10).ToList();

            var vm = new EmployeeLandingViewModels()
            {
                Employees = employees,
                PageNum = page
            };

            return PartialView("EmployeeLanding", vm);
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {

            if (!ModelState.IsValid)
                return View();

            if (emp.EmployeeId == 0)
            {
                var employee = new Employee
                {
                    Admin = emp.Admin,
                    EmployeeNumber = emp.EmployeeNumber,
                    FirstName = emp.FirstName,
                    SecondName = emp.SecondName
                };

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
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);

                employee.Admin = emp.Admin;
                employee.EmployeeNumber = emp.EmployeeNumber;
                employee.FirstName = emp.FirstName;
                employee.SecondName = emp.SecondName;
               
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
            var vm = new Employee();

            return PartialView("EmployeeForm", vm);
        }

        public ActionResult Edit(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            var vm = employee;
           

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
        
        public ActionResult Search(string str)
        {
            var employees = _context.Employees.ToList();

            var listToReturn = new List<Employee>();

            foreach (var emp in employees)
            {
                var fullName = emp.FirstName + emp.SecondName;
                if(fullName.ToUpper().Contains(str.ToUpper()))
                    listToReturn.Add(emp);
            }

            var vm = new EmployeeLandingViewModels()
            {
                Employees = listToReturn
            };

            return PartialView("EmployeeLanding", vm);

        }




        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}