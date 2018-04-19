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


        public List<Customer> Customers { get; set; }

        public int PageNum { get; set; }

        public bool IsSearch { get; set; }

    }
}