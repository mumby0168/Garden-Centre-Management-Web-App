using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Garden_Centre_MVC.Assets;
using Garden_Centre_MVC.Attributes;
using Garden_Centre_MVC.Attributes.Assets;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.EmployeeViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Garden_Centre_MVC.Controllers
{
    [NormalUser]
    [AdminUser]
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
            var vm = new EmployeeLandingViewModels {Employees = employees, PageNum = 1, IsSearch = false};

            //returns the home view
            return PartialView("EmployeeLanding", vm);
        }

        public ActionResult CheckAmountOfRecords()
        {
            var count = _context.Employees.Count();
            return Json(new {amount = count.ToString()});
        }

        public ActionResult LoadTablePage(int page)
        {
            var skipAmount = (page - 1) * 10;
            var employees = _context.Employees.OrderBy(e => e.EmployeeId).Skip(skipAmount).Take(10).ToList();
            var vm = new EmployeeLandingViewModels() {Employees = employees, PageNum = page};
            return PartialView("EmployeeLanding", vm);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Save(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                

                Error er = new Error()
                {
                    Property = emp,
                    ErrorMessage = "Employee Number Not Valid"
                };

                var json = JsonConvert.SerializeObject(er);


                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, json);
            }

            //new so add them to the database
            if (emp.EmployeeId == 0)
            {
                var employee = new Employee
                {
                    Admin = emp.Admin,
                    EmployeeNumber = emp.EmployeeNumber,
                    FirstName = emp.FirstName,
                    SecondName = emp.SecondName,
                    AccountCreated = false
                };
                _context.Employees.Add(employee);
                _context.SaveChanges();
                var vm = new EmployeeLandingViewModels() {Employees = _context.Employees.Take(10).ToList()};
                Logger.LogAction("Employee Added", employee.FirstName + employee.SecondName + "Added.");
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
                var vm = new EmployeeLandingViewModels() {Employees = _context.Employees.Take(10).ToList()};
                Logger.LogAction("Employee Edited", employee.FirstName + employee.SecondName + "Edited.");
                return PartialView("EmployeeLanding", vm);
            }
        }

        public ActionResult Remove(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee.EmployeeNumber == CurrentUser.EmployeeLogin.Employee.EmployeeNumber)
                return new HttpStatusCodeResult(500, "You cannot delete yourself as your are logged in.");
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            var vm = new EmployeeLandingViewModels() {Employees = _context.Employees.Take(10).ToList()};
            Logger.LogAction("Employee Deleted", employee.FirstName + employee.SecondName + "Deleted.");
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
        [System.Web.Mvc.HttpGet]
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
            EmployeeLandingViewModels vm;
            List<Employee> employees;

            if (str.IsNullOrWhiteSpace())
            {
                employees = _context.Employees.Take(10).ToList();

                vm = new EmployeeLandingViewModels()
                {
                    Employees = employees,
                    PageNum = 1,
                    IsSearch = false
                };
            }
            else
            {
                employees = _context.Employees.ToList();
                var listToReturn = new List<Employee>();
                foreach (var emp in employees)
                {
                    var fullName = emp.FirstName + emp.SecondName;
                    if (fullName.ToUpper().Contains(str.ToUpper())) listToReturn.Add(emp);
                }

                vm = new EmployeeLandingViewModels() { Employees = listToReturn, PageNum = 1, IsSearch = true };
            }

            return PartialView("EmployeeLanding", vm);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}