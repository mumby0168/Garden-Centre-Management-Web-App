using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Antlr.Runtime.Tree;

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

    }
    
}
