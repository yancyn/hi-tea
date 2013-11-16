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
    public class ChargeTest
    {
        private string connectionString = "DbLinqProvider=Sqlite;DbLinqConnectionType=System.Data.SQLite.SQLiteConnection, System.Data.SQLite, Version=1.0.66.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139;Data Source=pos.db3;";

        [Test]
        public void AddNoValueTest()
        {
            Main db = new Main(connectionString);
            Charge charge = new Charge();
            charge.Name = "Govern" + new Random().Next(20);
            db.Charges.InsertOnSubmit(charge);
            db.SubmitChanges();
        }
        [Test]
        public void AddValueTest()
        {
            Main db = new Main(connectionString);
            Charge charge = new Charge();
            charge.Name = "Tax" + new Random().Next(20);
            charge.Value = new Random().Next(100) / 13F;
            db.Charges.InsertOnSubmit(charge);
            db.SubmitChanges();
        }
    }
}
