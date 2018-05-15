using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace Garden_Centre_MVC.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Second Name")]
        public string SecondName { get; set; }

        public string FullName { get { return FirstName + " " + SecondName; } }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [MaxLength(255)]
        [Required]       
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        public bool CustomerDeleted { get; set; }
        
        [ScriptIgnore]
        public ICollection<Transaction> Transactions { get; set; }

        public Customer()
        {
            Transactions = new Collection<Transaction>();

        }
    }
}