using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Models
{
    /// <summary>
    /// This class is what will be used to creat the table in the database each property being a column in the table
    /// </summary>
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