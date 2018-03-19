using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int EmployeeNumber { get; set; }
    }
}