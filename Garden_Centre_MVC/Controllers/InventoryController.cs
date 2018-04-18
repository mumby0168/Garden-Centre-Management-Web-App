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

        //TODO: Come up with a better fix for this mess, it works like
        //this method ensures that the values of the transaction overviews are upto date when the item price changes
        public void UpdateTransactionOverviewValues(int itemId)
        {
            var affectedTransactions = m_Context.Transactions.Where(t => t.ItemId == itemId).ToList();
            List<int> affectedTransactionNumbers = new List<int>();
            foreach (Transaction t in affectedTransactions)
            {
                bool bAdd = true;
                if (affectedTransactionNumbers.Count > 0)
                {
                    foreach (int i in affectedTransactionNumbers)
                    {
                        if(i == t.TransactionNumber)
                        {
                            bAdd = false;
                            break;
                        }
                    }

                    if(bAdd)
                    {
                        affectedTransactionNumbers.Add(t.TransactionNumber);
                        continue;
                    }
                }
                else
                {
                    affectedTransactionNumbers.Add(t.TransactionNumber);
                    continue;
                }
            }

            foreach (int i in affectedTransactionNumbers)
            {
                var to = m_Context.TransactionOverviews.Where(t => t.TransactionNumber == i).First();
                to.TotalValue = 0;
                foreach(Transaction t in m_Context.Transactions.Where(a => a.TransactionNumber == i).ToList())
                {
                    to.TotalValue += m_Context.Items.Where(b => b.ItemId == t.ItemId).First().ItemPrice;
                }
            }

            m_Context.SaveChanges();
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

        public PartialViewResult EditItemView(int itemId)
        {
            Item vm = m_Context.Items.Where(i => i.ItemId == itemId).First();
            return PartialView("Partials/EditItemView", vm);
        }

        public PartialViewResult EditItemSerialize(Item editedItem)
        {
            var itemToUpdate = m_Context.Items.Where(i => i.ItemId == editedItem.ItemId).First();
            itemToUpdate.ItemPrice = editedItem.ItemPrice;
            itemToUpdate.OnOrder = editedItem.OnOrder;
            itemToUpdate.Sold = editedItem.Sold;
            itemToUpdate.Stock = editedItem.Stock;
            itemToUpdate.Description = editedItem.Description;

            m_Context.SaveChanges();

            UpdateTransactionOverviewValues(editedItem.ItemId);

            return InventoryView();
        }
    }
}