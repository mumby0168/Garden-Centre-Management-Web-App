using Garden_Centre_MVC.Assets;
using Garden_Centre_MVC.Attributes;
using Garden_Centre_MVC.Models;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.InventoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garden_Centre_MVC.Controllers
{
    /// <summary>
    /// This will be called by a ajax method which will all the data in the form it will then process it and decide whether the
    /// user can login if so return the home page view. If not then it shall return the next form with a error message.
    /// </summary>
    public class InventoryController : Controller, IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        /// <summary>
        /// Called to get the index page - not used as we use the InventoryView as a partial view
        /// </summary>
        [NormalUser]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This returns the view that lists all of the items in the inventory.
        /// </summary>
        /// <returns></returns>
        [NormalUser]
        public PartialViewResult InventoryView()
        {
            InventoryViewModel vm = new InventoryViewModel();
            return PartialView("Partials/InventoryView", vm);
        }

        /// <summary>
        /// This is called when a new item is created.
        /// It returns a form view.
        /// </summary>
        /// <returns></returns>
        [NormalUser]
        public PartialViewResult NewItemView()
        {
            InventoryViewModel vm = new InventoryViewModel();
            return PartialView("Partials/NewItemView");
        }

        /// <summary>
        /// This is called when the save button is clicked in the new item view.
        /// It is responsible for saving the data to the database.
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        [NormalUser]
        public PartialViewResult NewItemSerialize(Item newItem)
        {
            m_Context.Items.Add(newItem);
            m_Context.SaveChanges();
            Logger.LogAction("Item Added", "Added new item - " + newItem.Description);
            return InventoryView();
        }

        /// <summary>
        /// This is called when the user clicks edit on an item.
        /// It returns the edit view.
        /// </summary>
        /// <param name="editedItem"></param>
        [AdminUser]
        public PartialViewResult EditItemView(int itemId)
        {
            Item vm = m_Context.Items.Where(i => i.ItemId == itemId).First();
            return PartialView("Partials/EditItemView", vm);
        }

        /// <summary>
        /// This is called when the user clicks save on the edit item form.
        /// </summary>
        /// <param name="editedItem"></param>
        [AdminUser]
        public PartialViewResult EditItemSerialize(Item editedItem)
        {
            //update the item
            var itemToUpdate = m_Context.Items.Where(i => i.ItemId == editedItem.ItemId).First();
            itemToUpdate.OnOrder = editedItem.OnOrder;
            itemToUpdate.Stock = editedItem.Stock;
            itemToUpdate.Description = editedItem.Description;

            //Save the changes
            m_Context.SaveChanges();

            //Log that the item has been edited
            Logger.LogAction("Item Edited", "Edited item - " + editedItem.Description);

            return InventoryView();
        }
    }
}