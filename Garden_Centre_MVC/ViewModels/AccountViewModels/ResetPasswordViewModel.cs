using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.AccountViewModels
{
    /// <summary>
    /// This is a view model and it holds properties that can be set dynamically and this will relate to a view
    /// The view will take these properties and then display them as we choose in the .cshtml files.
    /// </summary>
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