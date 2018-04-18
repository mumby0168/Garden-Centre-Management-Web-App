using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garden_Centre_MVC.Models
{

    public class Log
    {
        public int LogId { get; set; }

        public Employee Employee { get; set; }

        public int EmployeeId { get; set; }

        public ActionType ActionType { get; set; }

        public int ActionTypeId { get; set; }

        public string PropertyEffected { get; set; }

        public DateTime DateOfAction { get; set; }
    }
}