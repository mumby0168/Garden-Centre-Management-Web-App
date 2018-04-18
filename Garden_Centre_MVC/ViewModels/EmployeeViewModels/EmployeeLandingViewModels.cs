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
        public EmployeeLandingViewModels()
        {
            IsSearch = false;
            PageNum = 1;
        }


        public List<Employee> Employees { get; set; }

        public int PageNum { get; set; }

       public bool IsSearch { get; set; }

    }
}