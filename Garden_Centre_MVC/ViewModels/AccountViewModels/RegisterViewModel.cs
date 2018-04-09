using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Attributes;

namespace Garden_Centre_MVC.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [CheckIfEmailExists]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Re-type Password")]
        public string ReTypePasssword { get; set; }

        [Required]
        [Display(Name = "Employee Number")]
        [CheckIfAlreadyRegistered]
        public int EmployeeNumber { get; set; }
    }
}