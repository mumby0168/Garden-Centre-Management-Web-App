using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.EmployeeViewModels
{
    /// <summary>
    /// This is a view model and it holds properties that can be set dynamically and this will relate to a view
    /// The view will take these properties and then display them as we choose in the .cshtml files.
    /// </summary>
    public class EmployeeLandingViewModels
    {
        /// <summary>
        /// This is the constructor and it will default some values for the view if they do not get set.
        /// </summary>
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