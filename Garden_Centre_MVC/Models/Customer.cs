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
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15)]
        public string SecondName { get; set; }

        public string FullName { get { return FirstName + " " + SecondName; } }

        [Required]
        [MaxLength(25)]
        public string AddressLine1 { get; set; }

        [Required]
        [MaxLength(15)]
        public string AddressLine2 { get; set; }

        [MaxLength(255)]
        [Required]
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