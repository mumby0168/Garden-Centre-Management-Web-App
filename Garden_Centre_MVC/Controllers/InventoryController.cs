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

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult InventoryView()
        {
            InventoryViewModel vm = new InventoryViewModel();
            return PartialView("Partials/InventoryView", vm);
        }

        public PartialViewResult NewItemView()
        {
            InventoryViewModel vm = new InventoryViewModel();
            return PartialView("Partials/NewItemView");
        }

        public PartialViewResult NewItemSerialize(Item newItem)
        {
            m_Context.Items.Add(newItem);
            m_Context.SaveChanges();

            return InventoryView();
        }
    }
}