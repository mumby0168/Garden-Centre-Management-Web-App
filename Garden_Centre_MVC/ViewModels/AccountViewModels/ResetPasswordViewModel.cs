using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Confirm Password")]
        public string ReTypePassword { get; set; }
    }
}