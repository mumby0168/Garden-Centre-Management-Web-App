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
    
     
    //Allows the class to be called throughout the application.
    public class CustomerController : Controller
    {
        private DatabaseContext _context;


        public CustomerController()
        {
            _context = new DatabaseContext();
        }

        //Class for action results, allows you to determine what result will form from an action that takes place.
        public ActionResult Index() 
        {
            //If the customer has not been deleted from the table, show 10 within the first page of the table.
            var customers = _context.Customers.Where(c => c.CustomerDeleted == false).Take(10).ToList(); 

            //Calls the CustomerLandingViewModel and sets the current properties.
            var vm = new CustomerLandingViewModels { Customers = customers, PageNum = 1, IsSearch = false };

            //Returns the home view.
            return PartialView("CustomerLanding", vm);
        }


        //Counts the amount of current records and then returns them in a string.
        public ActionResult CheckAmountOfRecords()
        {
            var count = _context.Customers.Count();

            return Json(new { amount = count.ToString() });
        }


        //Loads the Customers table.
        public ActionResult LoadTablePage(int page)
        {
            //Defines how many results to include within the page.
            var skipAmount = (page - 1) * 10;
            //Orders the Customers by their first name, and chooses customers to be included which have not been deleted. 10 is taken from the database to the list.
            var customers = _context.Customers.OrderBy(c => c.FirstName).Where(c => c.CustomerDeleted == false).Skip(skipAmount).Take(10).ToList();

            //Defines the customers and page from the view model.
            var vm = new CustomerLandingViewModels()
            {
                Customers = customers,
                PageNum = page
            };

            //Returns the home view.
            return PartialView("CustomerLanding", vm);
        }

        [HttpPost]
        public ActionResult Save(Customer cust) //Save method.
        {

            int errorCounter = 0; //Defining that the number of errors must equal 0.

            Error error = new Error(); //Initialising the object.
            error.ErrorMessages = new List<string>(); //The appropriate error message will be displayed as a string within the list.
            error.Property = cust;

            if (cust.FirstName.IsNullOrWhiteSpace() || cust.SecondName.IsNullOrWhiteSpace()) //If no text has been entered for the FirstName and SecondName:
            {
                error.ErrorMessages.Add("Please enter a first and last name."); //Display this error message as a string,
                errorCounter++; //And add 1 to the error counter.
            }

            if(cust.AddressLine1.IsNullOrWhiteSpace() || cust.AddressLine2.IsNullOrWhiteSpace()) //If no text has been entered for the AddressLine1 or AddressLine2:
            {
                error.ErrorMessages.Add("Please enter an Address Line 1 and 2."); //Display this error message as a string,
                errorCounter++; //And add 1 to the error counter.
            }

            if (cust.PostCode.IsNullOrWhiteSpace()) //If no text has been enteref for the PostCode:
            {
                error.ErrorMessages.Add("Please enter a postcode"); //Display this error message as a string,
                errorCounter++; //And add 1 to the error counter.
            }

            Regex reg = new Regex(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})"); //Regex string to provide validation for the possible ways/formats in which a postcode can be entered.


            if(cust.PostCode != null) //If some text has been entered for the Customer PostCode:
            {
                if (!reg.IsMatch(cust.PostCode)) //If the format is not a match to one that has been included within the regex string:
                {
                    error.ErrorMessages.Add("This is not a valid UK postcode"); //Display this error message as a string,
                    errorCounter++; //And add 1 to the error counter.
                }
            }
            


            if (errorCounter != 0) //If the error counter is not equal to 0:
            {
                //The expectation for 0 errors has failed.
                var obj = JsonConvert.SerializeObject(error); 
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, obj);
            }
            

            //Section to take in values which the customer has entered for the new ID.
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

                //Add the customer to the database and save the changes made.
                _context.Customers.Add(customer);
                _context.SaveChanges();

                //Show only customers which have not been deleted from the table, and take 10 to the list to show.
                var vm = new CustomerLandingViewModels()
                {
                    Customers = _context.Customers
                    .Where(c => c.CustomerDeleted == false)
                    .Take(10).
                    ToList()
                };


                //Add the change made to the Action log.
                Assets.Logger.LogAction("Customer Added", customer.FirstName + customer.SecondName + " Added.");


                //Return the home view.
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

                //Add to the action log the action of a customer being edited.
                Assets.Logger.LogAction("Customer Edited", customer.FirstName + customer.SecondName + " Edited.");

                //Return the view.
                return PartialView("CustomerLanding", vm);
            }
        }


        //Method for removing
        public ActionResult Remove(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);


            //If the customer has been deleted, save the database changes.
            customer.CustomerDeleted = true;

            
            _context.SaveChanges();

            var vm = new CustomerLandingViewModels()
            {
                Customers = _context.Customers
                .Where(c => c.CustomerDeleted == false)
                    .Take(10).
                    ToList()
            };


            //Log the change in the action logger of a customer being deleted.
            Assets.Logger.LogAction("Customer Deleted", customer.FirstName + customer.SecondName + " Deleted.");


            //Return the view.
            return PartialView("CustomerLanding", vm);
        }


        //Add button.
        public ActionResult Add()
        {
            var vm = new Customer();

            return PartialView("CustomerForm", vm); //Return the adding customer view model.
        }

        public ActionResult Edit(int id) //Edit button.
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            var vm = customer;


            return PartialView("CustomerForm", vm); //Reutnr the customer form view model.

        }

        public ActionResult GetAll() //Return the appropriate view.
        {
            return View();
        }

        public ActionResult GetSingle(int id) //Get the Id's and return view.
        {
            return View();
        }

        public ActionResult Search(string str) //Search method.
        {
            CustomerLandingViewModels vm;
            List<Customer> customers; //Display the strings of customers.

            if (str.IsNullOrWhiteSpace()) //If the customer has been deleted don't display them.
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
                var listToReturn = new List<Customer>(); //Show list of customers which have not been deleted.
                foreach (var cust in customers)
                {
                    var fullName = cust.FirstName + cust.SecondName;
                    if (fullName.ToUpper().Contains(str.ToUpper())) listToReturn.Add(cust);
                }

                
                vm = new CustomerLandingViewModels() { Customers = listToReturn, PageNum = 1, IsSearch = true };
            }

            //Return the view.
            return PartialView("CustomerLanding", vm);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}