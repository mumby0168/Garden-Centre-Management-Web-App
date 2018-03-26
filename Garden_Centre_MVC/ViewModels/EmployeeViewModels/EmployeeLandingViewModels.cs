using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.EmployeeViewModels
{
    public class EmployeeLandingViewModels
    {
        public List<Employee> Employees { get; set; }
    }
}