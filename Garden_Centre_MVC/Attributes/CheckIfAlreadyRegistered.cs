using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Persistance;
using Microsoft.Ajax.Utilities;

namespace Garden_Centre_MVC.Attributes
{
    public class CheckIfAlreadyRegistered : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("The Email has not been been provided.");
            }

            var context = new DatabaseContext();

            var check = context.EmployeeLogins
                .Include(e => e.Employee)
                .FirstOrDefault(e => e.Employee.EmployeeNumber == (int) value);

            if (check == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("This Id already has an account associated with it.");

        }
    }
}