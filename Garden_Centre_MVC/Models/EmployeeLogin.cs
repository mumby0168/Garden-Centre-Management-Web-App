using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Models
{
    public class EmployeeLogin
    {
        public int EmployeeLoginId { get; set; }

        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public Employee Employee { get; set; }

        public int EmployeeId { get; set; }

        public bool CanReset { get; set; }

    }
}