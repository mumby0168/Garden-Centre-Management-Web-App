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
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }
    }
}