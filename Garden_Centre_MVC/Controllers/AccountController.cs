using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Assets;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.AccountViewModels;

namespace Garden_Centre_MVC.Controllers
{
    public class AccountController : Controller
    {
        private DatabaseContext _context;

        public AccountController()
        {
            _context = new DatabaseContext();
        }
        
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginVm)
        {

            var employee = _context.EmployeeLogins.Include(e => e.Employee).FirstOrDefault(e => e.Username == loginVm.Email);

            if (employee == null)
                return View();

            if (Encryptor.Check(loginVm.Password, employee.Password, employee.Salt))
            {
                //succesful login
                CurrentUser.EmployeeLogin = employee;
                


                Session[employee.Username] = employee;

                return RedirectToAction("Index", "Home");
            }
                

            return View();
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult RegisterForm(RegisterViewModel registerVm)
        {
            if (registerVm.Password != registerVm.ReTypePasssword)
                return View("Register");

            var employeeLogins = _context.Employees.ToList();

            var employee = employeeLogins.FirstOrDefault(e => e.EmployeeNumber == registerVm.EmployeeNumber);

            if (employee == null)
                return View("Register");


            var returned = Encryptor.Encrypt(registerVm.Password);

            EmployeeLogin emp = new EmployeeLogin()
            {
                Username = registerVm.Username,
                Password = returned[0],
                Salt = returned[1],
                EmployeeId = employee.EmployeeId
            };

            _context.EmployeeLogins.Add(emp);
            _context.SaveChanges();

            var vm = new LoginViewModel()
            {
                Email = registerVm.Username,
                EmployeeNumber = registerVm.EmployeeNumber
            };
            
            return View("Login", vm);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult ForgotPassword()
        {

            return PartialView("ForgottenPassword");
        }
    }
}