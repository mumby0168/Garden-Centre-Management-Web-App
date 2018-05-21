using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.CustomerViewModels
{
    public class CustomerFormViewModel
    {
        public Customer Customer { get; set; } //Accessor methods for the Customer private field. 'Get' retrieves the information and returns it, before 'Set' writes it and makes it there visually.
    }
}