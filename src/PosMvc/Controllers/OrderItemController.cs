using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiTea.Pos;

namespace PosMvc.Controllers
{
    public class OrderItemController : Controller
    {
        private Main db = new Main(ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString);

        //
        // GET: /OrderItem/

        public ActionResult Index()
        {
            var items = db.OrderItems;
            return View(items);
        }

        /// <summary>
        /// List out only pending food order item.
        /// </summary>
        /// <returns></returns>
        public ActionResult PendingFood()
        {
            //SELECT Menu.Code, Menu.Name, OrderItem.*, 'Order'.*
            //FROM OrderItem
            //JOIN 'Order' ON OrderItem.ParentId = 'Order'.Id
            //JOIN Menu ON OrderItem.MenuId = Menu.Id
            //WHERE OrderItem.StatusId = 1 AND Menu.CategoryId IN(1,2,3)
            //ORDER BY OrderItem.Id, 'Order'.Id;

            int[] categories = new int[] { 2, 3, 4 };
            var pending = db.OrderItems.Where(i => i.StatusID == 1 && categories.Contains(i.Menu.CategoryID) && i.ParentID > 0);
            return View(pending.ToList());
        }
        /// <summary>
        /// List out only pending drink order item.
        /// </summary>
        /// <returns></returns>
        public ActionResult PendingDrink()
        {
            int[] categories = new int[] { 5, 6 };
            var pending = db.OrderItems.Where(i => i.StatusID == 1 && categories.Contains(i.Menu.CategoryID) && i.ParentID > 0);
            return View(pending.ToList());
        }

        //
        // GET: /OrderItem/Edit/5
        public ActionResult Edit(int id = 0)
        {
            OrderItem item = db.OrderItems.Where(i => i.ID == id).First();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /OrderItem/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderItem item)
        {
            if (ModelState.IsValid)
            {
                OrderItem existing = db.OrderItems.Where(i => i.ID == item.ID).First();
                existing.StatusID = item.StatusID;
                //System.Diagnostics.Debug.WriteLine("Updating " + existing.Menu.Name + " to " + item.StatusID);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

    }
}
