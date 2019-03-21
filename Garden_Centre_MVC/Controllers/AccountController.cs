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
    /// This class will be what handles all of the 
    /// </summary>
    public class AccountController : Controller
    {
        private DatabaseContext _context;

        /// <summary>
        /// this method is the contrcutor and will create a new instance of the database context
        /// </summary>
        public AccountController()
        {
            _context = new DatabaseContext();
        }
        
        /// <summary>
        /// This is the default method for the controller and will simply return the login view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var vm = new LoginViewModel();
            return View("Login", vm);
        }

        /// <summary>
        /// This will be called by a ajax method which will all the data in the form it will then process it and decide whether the
        /// user can login if so return the home page view. If not then it shall return the next form with a error message.
        /// </summary>
        /// <param name="loginVm"></param>
        /// <returns></returns>
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

                //Log log = new Log()
                //{
                //    EmployeeLogin = CurrentUser.EmployeeLogin,
                //    ActionType = _context.ActionTypes.FirstOrDefault(l => l.Description == "Logged In"),
                //    PropertyEffected = "None",
                //    DateOfAction = DateTime.Now
                //};

                //_context.Logs.Add(log);
                //_context.SaveChanges();
               
                Session[employee.Username] = employee;                

                return RedirectToAction("Index", "Home");
            }


            vm = new LoginViewModel() { ErrorMessage = errorMessage };
            return View("Login", vm);
        }      

        /// <summary>
        /// This method will return the register view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// This method will verify if a user can be registerd.
        /// if this is the case then it will return login view with the username prepopulated.
        /// If this is not the case it will return the screen with a error.
        ///</summary>
        /// <param name="registerVm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterForm(RegisterViewModel registerVm)
        {
            var users = _context.EmployeeLogins.ToList();
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

        /// <summary>
        /// this will return the forgotten password view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return PartialView("ForgottenPassword");
        }

        /// <summary>
        /// this method will send the email to the user in order to allow them to reset there password.
        /// if they are not in the system then it will not send the emaal.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult SendRecoveryEmail(ForgotPasswordViewModel vm)
        {       
            if (!ModelState.IsValid)
                return View();

            var employee = _context.EmployeeLogins.Include(m => m.Employee).FirstOrDefault(e => e.Username == vm.Email);

            if (employee == null)
                return PartialView("ForgottenPasswordError");

            if (employee.Employee.EmployeeNumber != vm.EmployeeId)
                return PartialView("ForgottenPasswordError");

            if (SendEmail(employee))
            {
                //email has been sent
                employee.CanReset = true;
                _context.SaveChanges();
                return View("ForgottenPasswordError");
            }
            else
            {
                //email has not been sent
                return View("Error");
            }


        }

        /// <summary>
        /// this will return the reset password view.
        /// </summary>
        /// <returns></returns>      
        public ActionResult Reset()
        {
            return View("ResetPassword");
        }

        /// <summary>
        /// this will check the reset form that is filled out by the user.
        /// this will also re-encrypt the password and then redirect the user to the login screen if they are succesful in registering in a account.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult CheckPasswordReset(ResetPasswordViewModel vm)
        {
            var employee = _context.EmployeeLogins.FirstOrDefault(e => e.Username == vm.Email);

            if (employee == null)
            {
                return View();
            }

            if (!employee.CanReset)
            {
                return Content("You need to send yourself a email in order to reset your password");

            }

            //check if the passwords match
            if (vm.Password != vm.ReTypePassword)
            {
                return View();
            }

            var bytes = Encryptor.Encrypt(vm.Password);

            employee.Password = bytes[0];
            employee.Salt = bytes[1];
            employee.CanReset = false;


            _context.SaveChanges();
            return Content("your password has been reset.");
        }


       /// <summary>
       /// this method will logoff the user as well as destroying there session.
       /// </summary>
       /// <returns></returns>
        public ActionResult Logoff()
        {       
            if (Session[CurrentUser.EmployeeLogin.Username] != null)
            {
                Logger.LogAction("Logged Out", "None.");

                Session.Contents.Remove(CurrentUser.EmployeeLogin.Username);
            }

            return RedirectToAction("Index", "Account");
        }

        #region Private Functions
        /// <summary>
        /// This method will use a STP server in order to send the user a email.
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This will dispose of the context in order to make sure that it is not a overhanging resource when the controller goes out of scope.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion



    }
}