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
        /// <summary>
        /// this method will allow a developer to log there actions in the controllers as they are made it will create  a record
        /// in the database when this method is called.
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="message"></param>
        /// <param name="empLog"></param>
        public static void LogAction(string actionType, string message, EmployeeLogin empLog = null)
        {
            //Log log;
            //DatabaseContext _context = new DatabaseContext();

            //if (empLog == null)
            //{
            //    log = new Log()
            //    {
            //        EmployeeLoginId = CurrentUser.EmployeeLogin.EmployeeLoginId,
            //        DateOfAction = DateTime.Now,
            //        ActionType = _context.ActionTypes.FirstOrDefault(a => a.Description == actionType),
            //        PropertyEffected = message
            //    };
            //}
            //else
            //{
            //    log = new Log()
            //    {
            //        EmployeeLoginId = empLog.EmployeeLoginId,
            //        DateOfAction = DateTime.Now,
            //        ActionType = _context.ActionTypes.FirstOrDefault(a => a.Description == actionType),
            //        PropertyEffected = message
            //    };
            //}
        
            //_context.Logs.Add(log);

            //_context.SaveChanges();
            //_context.Dispose();
        }
    }
}