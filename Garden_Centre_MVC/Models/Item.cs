using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Models
{
    /// <summary>
    /// This class is what will be used to creat the table in the database each property being a column in the table
    /// </summary>
    public class Item
    {
        public int ItemId { get; set; }

        [Required]
        public string Description { get; set; }

        public float ItemPrice { get; set; }

        public int Stock { get; set; }

        public int OnOrder { get; set; }

        public int Sold { get; set; }
    }
}