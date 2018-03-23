﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int TransactionNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}