using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using DbLinq.Data;
using DbLinq.Data.Linq;
using DbLinq.Sqlite;
using HiTea.Pos;
using NUnit.Framework;

namespace HiTea.Pos.NUnit
{
    [TestFixture]
    public class MenuTest
    {
        private string connectionString = "DbLinqProvider=Sqlite;DbLinqConnectionType=System.Data.SQLite.SQLiteConnection, System.Data.SQLite, Version=1.0.66.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139;Data Source=pos.db3;";

        [Test]
        public void AddTest()
        {
            Main db = new Main(connectionString);
            int expected = db.Menus.Count() + 1;

            HiTea.Pos.Menu menu = new HiTea.Pos.Menu();
            menu.Code = "T"+new Random().Next(20);
            menu.Name = "Egg"+new Random().Next(20);
            menu.Price = new Random().Next(100) / 13F;
            Category category = db.Categories.Where(c => c.Name == "Food").FirstOrDefault();
            if (category != null) menu.CategoryID = category.ID;
            db.Menus.InsertOnSubmit(menu);
            db.SubmitChanges();

            int actual = db.Menus.Count();
            Assert.AreEqual(expected, actual);
        }
    }
}
