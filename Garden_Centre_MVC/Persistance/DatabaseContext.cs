﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Persistance
{
    public class DatabaseContext : DbContext
    {


        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeLogin> EmployeeLogins { get; set; }

        public DbSet<Customer> Customers { get; set; }
         
        public DatabaseContext()
        {
            
        }
    }
}