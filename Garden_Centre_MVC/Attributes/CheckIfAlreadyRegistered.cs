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
    /// <summary>
    /// this is a attribute which can be placed on the top of a property and it will be checked in the
    /// entry of a form in a text field.
    /// </summary>
    public class CheckIfAlreadyRegistered : ValidationAttribute
    {
        /// <summary>
        /// this method will return a boolean and it will state whether or not the the id that has been given
        /// is already assigned to another employee.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
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