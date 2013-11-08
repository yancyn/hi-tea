using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    public class OrderTest
    {
        //private Main db;
        //private string connectionString = "DbLinqProvider=Sqlite;DbLinqConnectionType=System.Data.SQLite.SQLiteConnection, System.Data.SQLite, Version=1.0.66.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139;Data Source=pos.db3;";

        [SetUp]
        public void Initialize()
        {
            //db = new Main(connectionString);
        }

        private decimal Rounding(decimal original)
        {
            decimal result = 0m;
            decimal rounded = Math.Round(original, 1);
            System.Diagnostics.Debug.WriteLine(original + " => " + rounded);
            decimal half = rounded + 0.05m;

            decimal diff1 = rounded - original;
            if (diff1 < 0) diff1 = diff1 * -1;

            decimal diff2 = half - original;
            if (diff2 < 0) diff2 = diff2 * -1;

            System.Diagnostics.Debug.WriteLine(diff1 + " <> " + diff2);
            result = (diff1 > diff2) ? half : rounded;
            return result;
        }

        [Test]
        public void RoudUpTest()
        {
            decimal expected = 43.90m;
            decimal target = 43.88m;
            decimal actual = Rounding(target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RoudHalfTest()
        {
            decimal expected = 43.85m;
            decimal target = 43.83m;
            decimal actual = Rounding(target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RoudDownTest()
        {
            decimal expected = 43.80m;
            decimal target = 43.82m;
            decimal actual = Rounding(target);
            Assert.AreEqual(expected, actual);
        }


    }
}
