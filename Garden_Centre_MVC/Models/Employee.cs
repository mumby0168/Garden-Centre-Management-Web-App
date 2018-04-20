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
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15)]
        public string SecondName { get; set; }

        [Required]
        public int EmployeeNumber { get; set; }

        [Required]
        public bool Admin { get; set; }

        [Required]
        public bool AccountCreated { get; set; }

    }
    
}
