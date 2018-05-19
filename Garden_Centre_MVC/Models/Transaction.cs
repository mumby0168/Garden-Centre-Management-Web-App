using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Models
{
    /// <summary>
    /// This class is what will be used to creat the table in the database each property being a column in the table
    /// </summary>
    public class Transaction
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TransactionNumber { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}