using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        private int EmployeeNumber = 0;


        /// <summary>
        /// This is the constrcutor and it be called when the class is created.
        /// </summary>
        public EmployeeController()
        {
            _context = new DatabaseContext();
        }

        /// <summary>
        /// This is the default method for this controller it will load the employees out the database apply these to the
        /// view and then return it to the client.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var employees = _context.Employees.Take(10).ToList();
            var vm = new EmployeeLandingViewModels {Employees = employees, PageNum = 1, IsSearch = false};

            //returns the home view
            return PartialView("EmployeeLanding", vm);
        }

        /// <summary>
        /// This method is used by the client to establish how many records are in the database.
        /// This returns a new JSON object for us in Javascript.
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckAmountOfRecords()
        {
            var count = _context.Employees.Count();
            return Json(new {amount = count.ToString()});
        }

        /// <summary>
        /// this method is used when the the user either clicks the next or previous buttons on the paging.
        /// it will return the next 10 or the previous 10 records in the datbase depedning on what page of the table the user is on.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult LoadTablePage(int page)
        {
            var skipAmount = (page - 1) * 10;
            var employees = _context.Employees.OrderBy(e => e.EmployeeId).Skip(skipAmount).Take(10).ToList();
            var vm = new EmployeeLandingViewModels() {Employees = employees, PageNum = page};
            return PartialView("EmployeeLanding", vm);
        }

        /// <summary>
        /// This method is used when the user either edits or creates a new user. it will know whether the user is
        /// editing someone or creating a new one due to the value of the ID.
        ///it will validate each field and then serialise these results into a JSON object so the JS on the client can then
        /// display these to the user.
        /// If there no errors the new record or the edited one will be updated in the database.
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult Save(Employee emp)
        {

            int errorCounter = 0;
                
            Error error = new Error();
            error.ErrorMessages = new List<string>();
            error.Property = emp;

            Regex lettersOnly = new Regex("[a-z]");

            if (!emp.FirstName.IsNullOrWhiteSpace() || !emp.SecondName.IsNullOrWhiteSpace())
            {
                if (!lettersOnly.IsMatch(emp.FirstName) || !lettersOnly.IsMatch(emp.SecondName))
                {
                    error.ErrorMessages.Add("Please do not enter special characters or number in the name fields.");
                    errorCounter++;
                }

            }

            if (emp.FirstName.IsNullOrWhiteSpace() || emp.SecondName.IsNullOrWhiteSpace())
            {
                error.ErrorMessages.Add("Please enter a first and last name."); 
                errorCounter++;
            }

            if (errorCounter != 0 && emp.EmployeeId != 0)
            {
                var obj = JsonConvert.SerializeObject(error);
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, obj);
            }
            
            //new so add them to the database
            if (emp.EmployeeId == 0)
            {
                if (_context.Employees.FirstOrDefault(e => e.EmployeeNumber == emp.EmployeeNumber) != null)
                {
                    error.ErrorMessages.Add("The Employee Number has already been assigned.");

                    var obj = JsonConvert.SerializeObject(error);

                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, obj);
                }

                if (errorCounter != 0)
                {

                    if (emp.EmployeeNumber.ToString().Trim().Length != 6)
                    {
                        error.ErrorMessages.Add("The Employee number must be 6 digits.");
                        errorCounter++;
                    }

                    var obj = JsonConvert.SerializeObject(error);

                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, obj);
                }

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
                Logger.LogAction("Employee Added", employee.FirstName + employee.SecondName + " Added.");
                return View("EmployeeLanding", vm);
            }
            else if(errorCounter == 0)
            {
                //if the users id is already in the database
                if (_context.Employees.FirstOrDefault(e => e.EmployeeNumber == emp.EmployeeNumber) != null)
                {
                    if (emp.EmployeeNumber != EmployeeNumber)
                    {
                        error.ErrorMessages.Add("The Employee Number has already been assigned.");

                        var obj = JsonConvert.SerializeObject(error);

                        return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, obj);
                    }

                }

                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);
                employee.Admin = emp.Admin;
                employee.FirstName = emp.FirstName;
                employee.SecondName = emp.SecondName;
                _context.SaveChanges();
                var vm = new EmployeeLandingViewModels() {Employees = _context.Employees.Take(10).ToList()};
                Logger.LogAction("Employee Edited ", employee.FirstName + employee.SecondName + " Edited.");
                return PartialView("EmployeeLanding", vm);
            }

            var ob1j = JsonConvert.SerializeObject(error);
             
            return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ob1j);
        }

        /// <summary>
        /// this method will be used to remove a employee from the datbase.
        /// the main validation in this is making sure that the user cannot
        /// delete themselves is there are signed in as that employee.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// this will return the add view to the JS to display in the modal.
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var vm = new Employee();
            return PartialView("EmployeeForm", vm);
        }

        /// <summary>
        /// this will return the same add view but this time it shall be populated with the details
        /// of the employee who is to be edited.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            var vm = employee;

            EmployeeNumber = employee.EmployeeNumber;

            return PartialView("EmployeeForm", vm);
        }

        /// <summary>
        /// this method will be used in order to allow the user to search within the database.
        /// this will return the first 10 results that contain the string that the user has searched for.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// this will be called when the object is disposed of from by C# it will also get rid
        /// of the overhanging context object.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}