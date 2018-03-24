using System;
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

        public DbSet<Item> Items { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public List<TransactionOverview> GroupedTransactions
        {
            get
            {
                List<List<Transaction>> ret = new List<List<Transaction>>();
                //for every transaction
                for (int x = 0; x < Transactions.Include(m => m.Item).Include(s => s.Customer).ToList().Count; x++)
                {
                    //if the return is empty then add it anyways
                    if(ret.Count == 0)
                    {
                        ret.Add(new List<Transaction>());
                        ret[0].Add(Transactions.ToList()[x]);
                        continue;
                    }

                    bool bCreateNew = true;

                    //for all groups that are in return
                    for (int y = 0; y < ret.Count; y++)
                    {
                        if (ret[y][0].TransactionNumber == Transactions.ToList()[x].TransactionNumber)
                        {
                            ret[y].Add(Transactions.ToList()[x]);
                            bCreateNew = false;
                            break;
                        }
                    }

                    if(bCreateNew)
                    {
                        ret.Add(new List<Transaction>());
                        ret[ret.Count - 1].Add(Transactions.ToList()[x]);
                        continue;
                    }
                }

                List<TransactionOverview> retA = new List<TransactionOverview>();
                foreach(var collection in ret)
                {
                    retA.Add(new TransactionOverview(collection));
                }


                return retA;
            }
        }

        public DatabaseContext()
        {
            
        }
    }
}