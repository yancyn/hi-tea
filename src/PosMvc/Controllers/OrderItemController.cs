using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PosMvc.Controllers
{
    public class OrderItemController : Controller
    {
        private PosEntities db = new PosEntities();

        //
        // GET: /OrderItem/

        public ActionResult Index()
        {
            var orderitems = db.OrderItems.Include(o => o.Menu).Include(o => o.Order).Include(o => o.Status);
            return View(orderitems.ToList());
        }

        //
        // GET: /OrderItem/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            return View(orderitem);
        }

        //
        // GET: /OrderItem/Create

        public ActionResult Create()
        {
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Code");
            ViewBag.ParentId = new SelectList(db.Orders, "Id", "QueueNo");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            return View();
        }

        //
        // POST: /OrderItem/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderItem orderitem)
        {
            if (ModelState.IsValid)
            {
                db.OrderItems.Add(orderitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Code", orderitem.MenuId);
            ViewBag.ParentId = new SelectList(db.Orders, "Id", "QueueNo", orderitem.ParentId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", orderitem.StatusId);
            return View(orderitem);
        }

        //
        // GET: /OrderItem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Code", orderitem.MenuId);
            ViewBag.ParentId = new SelectList(db.Orders, "Id", "QueueNo", orderitem.ParentId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", orderitem.StatusId);
            return View(orderitem);
        }

        //
        // POST: /OrderItem/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderItem orderitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MenuId = new SelectList(db.Menus, "Id", "Code", orderitem.MenuId);
            ViewBag.ParentId = new SelectList(db.Orders, "Id", "QueueNo", orderitem.ParentId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", orderitem.StatusId);
            return View(orderitem);
        }

        //
        // GET: /OrderItem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            if (orderitem == null)
            {
                return HttpNotFound();
            }
            return View(orderitem);
        }

        //
        // POST: /OrderItem/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}