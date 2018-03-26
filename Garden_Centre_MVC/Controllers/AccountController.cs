using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Assets;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.AccountViewModels;

namespace Garden_Centre_MVC.Controllers
{
    /// <summary>
    /// Billy Mumby
    /// </summary>
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


        public ActionResult ForgotPassword()
        {
            return PartialView("ForgottenPassword");
        }

        public ActionResult SendRecoveryEmail(ForgotPasswordViewModel vm)
        {

            if (!ModelState.IsValid)
                return View();


            var employee = _context.EmployeeLogins.Include(m => m.Employee).FirstOrDefault(e => e.Username == vm.Email);

            if (employee.Employee.EmployeeNumber != vm.EmployeeId)
                return PartialView("ForgottenPassword");

            if (SendEmail(employee))
            {
                //email has been sent
                return View("ForgottenPassword");
            }
            else
            {
                //email has not been sent
                return View("ForgottenPassword");
            }


        }

        #region Private Functions

        private bool SendEmail(EmployeeLogin emp)
        {
            MailMessage msg = new MailMessage("greengardencentre@gmail.com", emp.Username)
            {
                Subject = "test",
                Body = "hello this is a test"
            };

            NetworkCredential creds = new NetworkCredential("greengardencentre@gmail.com", "hci12345");


            SmtpClient mailClient = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                UseDefaultCredentials = false,
                Credentials = creds,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            try
            {
                //try send email
                mailClient.Send(msg);
                Debug.WriteLine("Email has been sent");
                return true;
            }
            catch (Exception)
            {
                //if fails catch the exeception and return false for caller to handle
                return false;
            }            
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion



    }
}