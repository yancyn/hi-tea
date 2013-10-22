using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class CategoryTest
    {
        private string connectionString = "DbLinqProvider=Sqlite;DbLinqConnectionType=System.Data.SQLite.SQLiteConnection, System.Data.SQLite, Version=1.0.66.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139;Data Source=pos.db;";
        [Test]
        public void GetFoodCategoryTest()
        {
            Category expected = new Category();
            expected.Name = "Food";

            Main db = new Main(connectionString);
            Category actual = db.Category.Where(c => c.Name == "Food").FirstOrDefault();
            if(actual != null) System.Diagnostics.Debug.WriteLine(actual);
            Assert.IsTrue(expected.Name == actual.Name);
        }
    }
}
