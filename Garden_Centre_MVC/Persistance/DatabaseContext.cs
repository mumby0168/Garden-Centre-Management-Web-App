using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Garden_Centre_MVC.Models;
using MySql.Data.Entity;


namespace Garden_Centre_MVC.Persistance
{
    /// <summary>
    /// This the class that holds each object listed in the models folder the names of
    /// these propertys will be applied to the database as each table name.
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeLogin> EmployeeLogins { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionOverview> TransactionOverviews { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<ActionType> ActionTypes { get; set; }

        public DatabaseContext()
        {
            
        }
    }
}