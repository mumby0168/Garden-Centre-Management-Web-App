using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Attributes;
using Garden_Centre_MVC.Attributes.Assets;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.CustomerViewModels;

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
            var customers = _context.Customers.Take(10).ToList();

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
            var customers = _context.Customers.OrderBy(c => c.FirstName).Skip(skipAmount).Take(10).ToList();


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

            if (!ModelState.IsValid)
                return View();

            if (cust.CustomerId == 0)
            {
                var customer = new Customer
                {
                    FirstName = cust.FirstName,
                    SecondName = cust.SecondName,
                    AddressLine1 = cust.AddressLine1,
                    AddressLine2 = cust.AddressLine2,
                    PostCode = cust.PostCode
                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                var vm = new CustomerLandingViewModels()
                {
                    Customers = _context.Customers.Take(10).ToList()
                };

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
                    Customers = _context.Customers.Take(10).ToList()
                };

                return PartialView("CustomerLanding", vm);
            }
        }

        public ActionResult Remove(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            var vm = new CustomerLandingViewModels()
            {
                Customers = _context.Customers.Take(10).ToList()
            };

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
            var customers = _context.Customers.ToList();

            var listToReturn = new List<Customer>();

            foreach (var cust in customers)
            {
                var fullName = cust.FirstName + cust.SecondName;
                if (fullName.ToUpper().Contains(str.ToUpper()))
                    listToReturn.Add(cust);
            };

            var vm = new CustomerLandingViewModels()
            {
                Customers = listToReturn,
                PageNum = 1,
                IsSearch = true
            };

            return PartialView("CustomerLanding", vm);

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}