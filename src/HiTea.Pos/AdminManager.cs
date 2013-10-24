using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    /// <summary>
    /// Admin manager class to create all basic food menu required.
    /// </summary>
    public class AdminManager
    {
        protected Main db;
        public ObservableCollection<User> Users { get; set; }
        //public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }
        public ObservableCollection<Charge> Charges { get; set; }

        public AdminManager()
        {
            this.Users = new ObservableCollection<User>();
            this.Charges = new ObservableCollection<Charge>();
            this.Menus = new ObservableCollection<Menu>();

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            db = new Main(connectionString);

            // dump users
            foreach (var user in db.Users)
                this.Users.Add(user);

            foreach (var menu in db.Menus)
                this.Menus.Add(menu);

            foreach (var charge in db.Charges)
                this.Charges.Add(charge);

            // TODO: retrieve categoris and food menu
            //this.Categories = new ObservableCollection<Category>();
            //foreach (var category in db.Categories)
            //{
            //    foreach (var menu in category.Menus)
            //        category.MenuCollection.Add(menu);
            //    this.Categories.Add(category);
            //}
        }
    }
}
