using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.LogViewModels
{
    /// <summary>
    /// This is a view model and it holds properties that can be set dynamically and this will relate to a view
    /// The view will take these properties and then display them as we choose in the .cshtml files.
    /// </summary>
    public class LogLandingViewModel
    {
        public List<Log> Logs { get; set; }
    }
}