using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.CustomerViewModels;

namespace Garden_Centre_MVC.Controllers
{
    public class CustomersController : Controller
    {
        private DatabaseContext _context;

        public CustomersController()
        {
            _context = new DatabaseContext();
        }


        // GET: Customers
        public ActionResult Index()
        {
            var customers = _context.Customers.ToList();

            var vm = new CustomerLandingViewModels {Customers = customers};

            //returns the home view
            return View("CustomerLanding", vm);
        }

        public ActionResult Save(int id)
        {
            if (id == 0)
            {
                //then add the customer as they must be new
                return View();
            }
            else
            {
                //if not then we need to edit the customer record [Use Automapper]
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.FirstOrDefault(e => e.CustomerId == id);

            var vm = new CustomerFormViewModel()
            {
                Customer = customer
            };

            return PartialView("CustomerForm", vm);

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