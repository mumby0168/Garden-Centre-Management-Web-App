using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.CustomerViewModels
{
    public class CustomerLandingViewModels
    {
        public CustomerLandingViewModels()
        {
            IsSearch = false;
            PageNum = 1;
        }


        public List<Customer> Customers { get; set; } //Accessor methods for the Customers private field. 'Get' reads the information and returns it, 'Set' writes the information in that private field.

        public int PageNum { get; set; } //Accessor methods for the PageNum private field.

        public bool IsSearch { get; set; } //Accessor methods for the IsSearch private field.

    }
}