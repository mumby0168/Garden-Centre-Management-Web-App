using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Antlr.Runtime.Tree;
using Garden_Centre_MVC.Attributes;

namespace Garden_Centre_MVC.Models
{
    /// <summary>
    /// This class is what will be used to creat the table in the database each property being a column in the table
    /// </summary>
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
