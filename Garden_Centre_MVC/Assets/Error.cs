using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Assets
{
    /// <summary>
    /// this object is used in order to create the list off errors that are used in the
    /// customer and employee pages.
    /// </summary>
    public class Error
    {
        public object Property { get; set; }

        public List<string> ErrorMessages { get; set; }
    }
}