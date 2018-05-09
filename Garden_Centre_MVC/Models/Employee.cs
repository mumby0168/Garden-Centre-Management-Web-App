using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Antlr.Runtime.Tree;
using Garden_Centre_MVC.Attributes;

namespace Garden_Centre_MVC.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Second Name")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Employee Number")]
        public int EmployeeNumber { get; set; }

        [Required]
        [Display(Name = "Is Admin?")]
        public bool Admin { get; set; }

        [Required]
        public bool AccountCreated { get; set; }

    }
    
}
