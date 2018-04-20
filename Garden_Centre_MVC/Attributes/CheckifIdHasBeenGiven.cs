using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Garden_Centre_MVC.Persistance;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace Garden_Centre_MVC.Attributes
{
    public class CheckifIdHasBeenGiven : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DatabaseContext _context = new DatabaseContext();

            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == (int) value);

            if (employee != null)
            {
                return new ValidationResult("This Employee Number has already been assigned.");
            }

            return ValidationResult.Success;

        }
    }
}