using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;

namespace Garden_Centre_MVC.ViewModels.Transactions
{
    public class AddViewModel : IDisposable
    {
        public AddViewModel()
        {
            m_Items = new List<Item>();
        }

        public Customer _Customer
        {
            get; set;
        }

        public Object AddItem()
        {
            foreach (Item item in m_Context.Items.ToList())
            {
                if(item.ItemId == _Item)
                {
                    System.Diagnostics.Debug.WriteLine(_Item);
                    m_Items.Add(item);
                }
            }

            return null;
        }

        public int _Item
        {
            get; set;
        }

        private List<Item> m_Items = null;
        public List<Item> _Items
        {
            get
            {
                return m_Items;
            }
        }

        private DatabaseContext m_Context = new DatabaseContext();

        public List<Customer> Customers
        {
            get
            {

                return m_Context.Customers.ToList();
            }
        }

        public List<Item> Items
        {
            get
            {
                return m_Context.Items.ToList();
            }
        }

        public void Dispose()
        {
            m_Context.Dispose();
        }
    }
}