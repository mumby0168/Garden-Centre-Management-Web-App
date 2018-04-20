using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Assets
{
    public class Error
    {
        public object Property { get; set; }

        public List<string> ErrorMessages { get; set; }
    }
}