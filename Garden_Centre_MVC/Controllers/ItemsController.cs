using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garden_Centre_MVC.Persistance;
using Garden_Centre_MVC.ViewModels.ItemsViewModels;

namespace Garden_Centre_MVC.Controllers
{
    public class ItemsController : Controller
    {
        private DatabaseContext _context;

        public ItemsController()
        {
            _context = new DatabaseContext();
        }
        // GET: Items
        public ActionResult Index()
        {
            var items = _context.Items.Take(10).ToList();

            var vm = new ItemsLandingPageViewModel()
            {
                Items = items
            };

            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}