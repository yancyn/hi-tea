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
    public class UtilsTest
    {
        [Test]
        public void RoudUpTest()
        {
            decimal expected = 43.90m;
            decimal target = 43.88m;
            decimal actual = Utils.Rounding(target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RoudHalfTest()
        {
            decimal expected = 43.85m;
            decimal target = 43.83m;
            decimal actual = Utils.Rounding(target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RoudDownTest()
        {
            decimal expected = 43.80m;
            decimal target = 43.82m;
            decimal actual = Utils.Rounding(target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoRoundTest()
        {
            decimal expected = 25.65m;
            decimal target = 25.652m;
            decimal actual = Utils.Rounding(target);
            Assert.AreEqual(expected, actual);
        }
    }
}
