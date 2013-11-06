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
    public class MenuTest
    {
        private Main db;
        private string connectionString = "DbLinqProvider=Sqlite;DbLinqConnectionType=System.Data.SQLite.SQLiteConnection, System.Data.SQLite, Version=1.0.66.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139;Data Source=pos.db3;";
        private string[] foods = new string[]{"Egg", "Hotdog", "Soup", "Cake", "Rice", "Soya", "Tomato", "Beer"};

        [SetUp]
        public void Initialize()
        {
            db = new Main(connectionString);
        }

        private void Add(string code, string cat)
        {
            int expected = db.Menus.Count() + 1;

            HiTea.Pos.Menu menu = new HiTea.Pos.Menu();
            menu.Code = code + new Random().Next(20);
            menu.Name = foods[new Random().Next(foods.Length)] + new Random().Next(20);
            menu.Price = (float)Math.Round(new Random().Next(100) / 13F, 2);
            menu.Active = true;
            Category category = db.Categories.Where(c => c.Name == cat).FirstOrDefault();
            if (category != null) menu.CategoryID = category.ID;
            db.Menus.InsertOnSubmit(menu);
            db.SubmitChanges();

            int actual = db.Menus.Count();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddSetMealTest()
        {
            Add("S", "Set Meal");
        }
        [Test]
        public void AddFoodTest()
        {
            Add("F", "Food");
        }
        [Test]
        public void AddBeverageTest()
        {
            Add("D", "Drink");
        }
        [Test]
        public void AddDessertTest()
        {
            Add("E", "Dessert");
        }

        [Test]
        public void MenuNameTest()
        {
            Menu menu = new Menu();
            menu.Code = "A01";
            menu.Name = "上海叉烧鸡饭 SHANGHAI BBQ CHICKEN RICE";

            string expected = "A01 上海叉烧鸡饭\nSHANGHAI BBQ CHICKEN RICE";
            string actual = string.Empty;
            actual += menu.Code + " ";

            // extract English character only
            string eng = string.Empty;
            //Match match = Regex.Match(menu.Name, "[a-zA-Z0-9 ]");
            Regex regex = new Regex("[a-zA-Z0-9 ]");
            foreach (Match match in regex.Matches(menu.Name))
                eng += match.Value;
            string other = menu.Name.Replace(eng, string.Empty);
            actual += other.Trim() + "\n" + eng.Trim();

            System.Diagnostics.Debug.WriteLine("Eng: " + eng);
            System.Diagnostics.Debug.WriteLine("Other: " + other);            
            Assert.AreEqual(expected, actual);
        }


    }
}
