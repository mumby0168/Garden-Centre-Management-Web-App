using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Garden_Centre_MVC.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }   

        public string PostCode { get; set; }


    }
}