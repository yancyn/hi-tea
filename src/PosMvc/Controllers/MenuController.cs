using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiTea.Pos;

namespace PosMvc.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db = new Main(connectionString);
            var menus = db.Menus;
            return View(menus.ToList());
        }

    }
}
