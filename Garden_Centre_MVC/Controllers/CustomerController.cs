using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Assets;
using Garden_Centre_MVC.Attributes;
using Garden_Centre_MVC.Attributes.Assets;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.CustomerViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Garden_Centre_MVC.Controllers
{
    
     
    
    public class CustomerController : Controller
    {
        private DatabaseContext _context;


        public CustomerController()
        {
            _context = new DatabaseContext();
        }
        
        public ActionResult Index()
        {
            var customers = _context.Customers.Where(c => c.CustomerDeleted == false).Take(10).ToList();

            var vm = new CustomerLandingViewModels { Customers = customers, PageNum = 1, IsSearch = false };

            //returns the home view
            return PartialView("CustomerLanding", vm);
        }

        public ActionResult CheckAmountOfRecords()
        {
            var count = _context.Customers.Count();

            return Json(new { amount = count.ToString() });
        }



        public ActionResult LoadTablePage(int page)
        {
            var skipAmount = (page - 1) * 10;
            var customers = _context.Customers.OrderBy(c => c.FirstName).Where(c => c.CustomerDeleted == false).Skip(skipAmount).Take(10).ToList();


            var vm = new CustomerLandingViewModels()
            {
                Customers = customers,
                PageNum = page
            };

            return PartialView("CustomerLanding", vm);
        }

        [HttpPost]
        public ActionResult Save(Customer cust)
        {

            int errorCounter = 0;

            Error error = new Error();
            error.ErrorMessages = new List<string>();
            error.Property = cust;

            if (cust.FirstName.IsNullOrWhiteSpace() || cust.SecondName.IsNullOrWhiteSpace())
            {
                error.ErrorMessages.Add("Please enter a first and last name.");
                errorCounter++;
            }

            if(cust.AddressLine1.IsNullOrWhiteSpace() || cust.AddressLine2.IsNullOrWhiteSpace())
            {
                error.ErrorMessages.Add("Please enter an Address Line 1 and 2.");
                errorCounter++;
            }

            if (cust.PostCode.IsNullOrWhiteSpace())
            {
                error.ErrorMessages.Add("Please enter a postcode");
                errorCounter++;
            }

            Regex reg = new Regex(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})");


            if(cust.PostCode != null)
            {
                if (!reg.IsMatch(cust.PostCode))
                {
                    error.ErrorMessages.Add("This is not a valid UK postcode");
                    errorCounter++;
                }
            }
            


            if (errorCounter != 0)
            {
                var obj = JsonConvert.SerializeObject(error);
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, obj);
            }
               
            if (cust.CustomerId == 0)
            {
                var customer = new Customer
                {
                    FirstName = cust.FirstName,
                    SecondName = cust.SecondName,
                    AddressLine1 = cust.AddressLine1,
                    AddressLine2 = cust.AddressLine2,
                    PostCode = cust.PostCode,
                    CustomerDeleted = false
                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                var vm = new CustomerLandingViewModels()
                {
                    Customers = _context.Customers
                    .Where(c => c.CustomerDeleted == false)
                    .Take(10).
                    ToList()
                };

                Assets.Logger.LogAction("Customer Added", customer.FirstName + customer.SecondName + " Added.");

                return View("CustomerLanding", vm);
            }
            else
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == cust.CustomerId);

                customer.FirstName = cust.FirstName;
                customer.SecondName = cust.SecondName;
                customer.AddressLine1 = cust.AddressLine1;
                customer.AddressLine2 = cust.AddressLine2;
                customer.PostCode = cust.PostCode;

                _context.SaveChanges();


                var vm = new CustomerLandingViewModels()
                {
                    Customers = _context.Customers.Take(10).Where(c => c.CustomerDeleted == false).ToList()
                };

                Assets.Logger.LogAction("Customer Edited", customer.FirstName + customer.SecondName + " Edited.");

                return PartialView("CustomerLanding", vm);
            }
        }

        public ActionResult Remove(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            customer.CustomerDeleted = true;

            
            _context.SaveChanges();

            var vm = new CustomerLandingViewModels()
            {
                Customers = _context.Customers
                .Where(c => c.CustomerDeleted == false)
                    .Take(10).
                    ToList()
            };

            Assets.Logger.LogAction("Customer Deleted", customer.FirstName + customer.SecondName + " Deleted.");

            return PartialView("CustomerLanding", vm);
        }

        public ActionResult Add()
        {
            var vm = new Customer();

            return PartialView("CustomerForm", vm);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            var vm = customer;


            return PartialView("CustomerForm", vm);

        }

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
            CustomerLandingViewModels vm;
            List<Customer> customers;

            if (str.IsNullOrWhiteSpace())
            {
                customers = _context.Customers
                .Where(c => c.CustomerDeleted == false)
                    .Take(10).
                    ToList();

                vm = new CustomerLandingViewModels()
                {
                    Customers = customers,
                    PageNum = 1,
                    IsSearch = false
                };
            }
            else
            {
                customers = _context.Customers.Where(c => c.CustomerDeleted == false).ToList();
                var listToReturn = new List<Customer>();
                foreach (var cust in customers)
                {
                    var fullName = cust.FirstName + cust.SecondName;
                    if (fullName.ToUpper().Contains(str.ToUpper())) listToReturn.Add(cust);
                }

                vm = new CustomerLandingViewModels() { Customers = listToReturn, PageNum = 1, IsSearch = true };
            }

            return PartialView("CustomerLanding", vm);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}