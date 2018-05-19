using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Models
{
    /// <summary>
    /// This class is what will be used to creat the table in the database each property being a column in the table
    /// </summary>
    public class Log
    {
        public int LogId { get; set; }

        public EmployeeLogin EmployeeLogin { get; set; }

        public int EmployeeLoginId { get; set; }

        public ActionType ActionType { get; set; }

        public int ActionTypeId { get; set; }

        public string PropertyEffected { get; set; }

        public DateTime DateOfAction { get; set; }
    }
}