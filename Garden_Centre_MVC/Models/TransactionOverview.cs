using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Models
{
    public class TransactionOverview
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TransactionNumber { get; set; }
        [Required]
        public float TotalValue { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public String FullName { get { return "REPLACE ME"; } } 
    }
}