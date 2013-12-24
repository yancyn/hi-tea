using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiTea.Pos;

namespace PosMvc.Controllers
{
    public class OrderController : Controller
    {
        private Main db = new Main(ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString);

        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        //
        // GET: /Order/Create
        public ActionResult Create()
        {
            Order order = new Order();
            return View(order);
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.InsertOnSubmit(order);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        //
        // GET: /Order/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Order order = db.Orders.Where(o => o.ID == id).FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // TODO: POST: /Stock/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(stock).State = EntityState.Modified;
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

    }
}
