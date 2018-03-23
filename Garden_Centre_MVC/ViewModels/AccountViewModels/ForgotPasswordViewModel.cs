using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Employee Id")]
        [MinLength(6, ErrorMessage = "The ID needs to be 6 digits")]
        [MaxLength(6, ErrorMessage = "The ID is greater than 6 digits")]
        public int EmployeeId { get; set; }
    }
}