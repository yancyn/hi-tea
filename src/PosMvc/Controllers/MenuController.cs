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
        private Main db = new Main(ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString);

        //
        // GET: /Menu/

        public ActionResult Index()
        {
            var menus = db.Menus;
            return View(menus.ToList());
        }

    }
}
