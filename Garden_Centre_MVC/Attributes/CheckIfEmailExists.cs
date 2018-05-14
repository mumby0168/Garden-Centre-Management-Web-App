using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Persistance;

namespace Garden_Centre_MVC.Attributes
{
    public class CheckIfEmailExists : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
               return new ValidationResult("The Email has not been provided.");
            }

            var context = new DatabaseContext();

            

            var check = context.EmployeeLogins.FirstOrDefault(e => e.Username == value.ToString());

            context.Dispose();

            if (check == null)
                return ValidationResult.Success;


            return new ValidationResult("This username is already in use.");

        }

        
    }
}