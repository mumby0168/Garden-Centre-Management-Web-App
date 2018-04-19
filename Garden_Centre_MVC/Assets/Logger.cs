using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Attributes.Assets;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;

namespace Garden_Centre_MVC.Assets
{
    public static class Logger
    {
        public static void LogAction(string ActionType, string message)
        {
            DatabaseContext _context = new DatabaseContext();

            Log log = new Log()
            {
                EmployeeLogin = CurrentUser.EmployeeLogin,
                DateOfAction = DateTime.Now,
                ActionType = _context.ActionTypes.FirstOrDefault(a => a.Description == ActionType),
                PropertyEffected = message
            };

            _context.Logs.Add(log);

            _context.SaveChanges();
            _context.Dispose();


        }
    }
}