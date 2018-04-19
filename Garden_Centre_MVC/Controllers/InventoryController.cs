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
    public class InventoryController : Controller, IDisposable
    {
        private DatabaseContext m_Context = new DatabaseContext();
        public void Dispose()
        {
            m_Context.Dispose();
        }

        [NormalUser]
        public ActionResult Index()
        {
            return View();
        }

        [NormalUser]
        public PartialViewResult InventoryView()
        {
            InventoryViewModel vm = new InventoryViewModel();
            return PartialView("Partials/InventoryView", vm);
        }

        [NormalUser]
        public PartialViewResult NewItemView()
        {
            InventoryViewModel vm = new InventoryViewModel();
            return PartialView("Partials/NewItemView");
        }

        [NormalUser]
        public PartialViewResult NewItemSerialize(Item newItem)
        {
            m_Context.Items.Add(newItem);
            m_Context.SaveChanges();

            return InventoryView();
        }

        [AdminUser]
        public PartialViewResult EditItemView(int itemId)
        {
            Item vm = m_Context.Items.Where(i => i.ItemId == itemId).First();
            return PartialView("Partials/EditItemView", vm);
        }

        [AdminUser]
        public PartialViewResult EditItemSerialize(Item editedItem)
        {
            var itemToUpdate = m_Context.Items.Where(i => i.ItemId == editedItem.ItemId).First();
            itemToUpdate.ItemPrice = editedItem.ItemPrice;
            itemToUpdate.OnOrder = editedItem.OnOrder;
            itemToUpdate.Stock = editedItem.Stock;
            itemToUpdate.Description = editedItem.Description;

            m_Context.SaveChanges();

            return InventoryView();
        }
    }
}