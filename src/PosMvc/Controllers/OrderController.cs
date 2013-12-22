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

    }
}
