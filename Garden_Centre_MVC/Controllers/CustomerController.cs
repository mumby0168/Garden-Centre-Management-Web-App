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

        /// <summary>
        /// gets all of the customers to be viewed in the table
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