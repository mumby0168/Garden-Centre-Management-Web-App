using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public float ItemPrice { get; set; }

        public int Stock { get; set; }

        public int OnOrder { get; set; }

        public int Sold { get; set; }
    }
}