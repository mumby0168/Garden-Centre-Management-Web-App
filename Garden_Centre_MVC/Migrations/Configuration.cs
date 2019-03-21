using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garden_Centre_MVC.Persistance.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Garden_Centre_MVC.Persistance.DatabaseContext context)
        {
            var employee = context.Employees.FirstOrDefault(e => e.EmployeeNumber == 123456);

            if (employee == null)
            {
                var employeeLogin = new EmployeeLogin()
                {
                    Username = "joebloggs@outlook.com",
                    Employee = new Employee()
                    {
                        FirstName = "Joe",
                        SecondName = "Bloggs",
                        EmployeeNumber = 123456,
                        Admin = true,
                        AccountCreated = false
                    }
                };
                context.EmployeeLogins.Add(employeeLogin);
            }

            context.SaveChanges();
        }
    }
}
