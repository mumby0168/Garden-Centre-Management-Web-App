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
    /// <summary>
    /// this is a attribute which can be placed on the top of a property and it will be checked in the
    /// entry of a form in a text field.
    /// </summary>
    public class CheckIfEmailExists : ValidationAttribute
    {
        /// <summary>
        /// this method returns a boolean and will state whether or not someone is already registered with the email
        /// if this is the case then they will be sent back a error message within the form.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
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