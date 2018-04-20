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
using Garden_Centre_MVC.Attributes.Assets;
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
            var vm = new LoginViewModel();
            return View("Login", vm);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginVm)
        {
            var errorMessage = "THIS LOGIN IS INCORRECT OR DOES NOT EXIST, TRY AGAIN";
            LoginViewModel vm;
            var employee = _context.EmployeeLogins.Include(e => e.Employee).FirstOrDefault(e => e.Username == loginVm.Email);

            if (employee == null)
            {
                vm = new LoginViewModel() { ErrorMessage = errorMessage };
                return View("Login", vm);
            }
            if (employee.Employee.EmployeeNumber != loginVm.EmployeeNumber)
            {
                vm = new LoginViewModel() {ErrorMessage = errorMessage};
                return View("Login", vm);
            }
            if (Encryptor.Check(loginVm.Password, employee.Password, employee.Salt))
            {
                //succesful login
                CurrentUser.EmployeeLogin = employee;

                Log log = new Log()
                {
                    EmployeeLogin = CurrentUser.EmployeeLogin,
                    ActionType = _context.ActionTypes.FirstOrDefault(l => l.Description == "Logged In"),
                    PropertyEffected = "None",
                    DateOfAction = DateTime.Now
                };

                _context.Logs.Add(log);
                _context.SaveChanges();
               
                Session[employee.Username] = employee;                

                return RedirectToAction("Index", "Home");
            }


            vm = new LoginViewModel() { ErrorMessage = errorMessage };
            return View("Login", vm);
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult RegisterForm(RegisterViewModel registerVm)
        {
            var checkifExist = _context.EmployeeLogins.FirstOrDefault(e => e.Username == registerVm.Email);

            if (checkifExist != null)
                return View("Register");


            if (registerVm.Password != registerVm.ReTypePasssword)
                return View("Register");


            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == registerVm.EmployeeNumber);

            if (employee == null)
                return View("Register");

            if (employee.AccountCreated)
                return View("Register");

            var returned = Encryptor.Encrypt(registerVm.Password);

            EmployeeLogin emp = new EmployeeLogin()
            {
                Username = registerVm.Email,
                Password = returned[0],
                Salt = returned[1],
                EmployeeId = employee.EmployeeId,
                CanReset = false
            };

            _context.EmployeeLogins.Add(emp);

            employee.AccountCreated = true;

            _context.SaveChanges();

            var vm = new LoginViewModel()
            {
                Email = registerVm.Email,
                EmployeeNumber = registerVm.EmployeeNumber
            };

            Logger.LogAction("Registered", "None.", emp);
            
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

            if (employee == null)
                return PartialView("ForgottenPassword");

            if (employee.Employee.EmployeeNumber != vm.EmployeeId)
                return PartialView("ForgottenPassword");

            if (SendEmail(employee))
            {
                //email has been sent
                employee.CanReset = true;
                _context.SaveChanges();
                return View("EmailSent");
            }
            else
            {
                //email has not been sent
                return View("ForgottenPassword");
            }


        }

        public ActionResult ResetPassword()
        {
            return View("ResetPasswordView");
        }

        public ActionResult Reset()
        {
            return View("ResetPassword");
        }

        //TODO: this needs testing and we need to think of a way to send a link out via email and verify that it is the user. 
        public ActionResult CheckPasswordReset(ResetPasswordViewModel vm)
        {
            var employee = _context.EmployeeLogins.FirstOrDefault(e => e.Username == vm.Email);

            if (employee == null)
            {
                return View();
            }

            if (!employee.CanReset)
            {
                return View();

            }

            //check if the passwords match
            if (vm.Password != vm.ReTypePassword)
            {
                return View();
            }

            var bytes = Encryptor.Encrypt(vm.Password);

            employee.Password = bytes[0];
            employee.Salt = bytes[1];


            _context.SaveChanges();
            return Content("your password has been reset.");
        }

        public ActionResult Logoff()
        {

            if (Session[CurrentUser.EmployeeLogin.Username] != null)
            {
                Log log = new Log()
                {
                    EmployeeLogin = CurrentUser.EmployeeLogin,
                    ActionType = _context.ActionTypes.FirstOrDefault(a => a.Description == "Logged Out"),
                    PropertyEffected = "None",
                    DateOfAction = DateTime.Now
                };
                Session.Contents.Remove(CurrentUser.EmployeeLogin.Username);
            }

            return RedirectToAction("Index", "Account");
        }

        #region Private Functions

        private bool SendEmail(EmployeeLogin emp)
        {

            MailMessage msg = new MailMessage("greengardencentre@gmail.com", emp.Username)
            {
                Subject = "Password Recovery",
                Body = "Hello " + emp.Username + Environment.NewLine + " Please use the following link to reset your account:  http://localhost:56163/Account/Reset "
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