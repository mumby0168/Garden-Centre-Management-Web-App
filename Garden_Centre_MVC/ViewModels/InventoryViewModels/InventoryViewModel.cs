using Garden_Centre_MVC.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.ViewModels.InventoryViewModels
{
    /// <summary>
    /// This view model represents the inventory class
    /// </summary>
    public class InventoryViewModel : IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        //Returns the list of items that makes up the inventory from the database
        public List<Item> inventory
        {
            get
            {
                return m_Context.Items.ToList();
            }
        }

        //Returns a JSON version of the inventory 
        public string inventoryJSON
        {
            get
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(inventory);
            }
        }
    }
}