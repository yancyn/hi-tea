using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class PosManager
    {
        protected Main db;
        public ObservableCollection<Category> Categories { get; set; }

        public PosManager()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            db = new Main(connectionString);

            this.Categories = new ObservableCollection<Category>();
            foreach (var category in db.Categories)
            {
                foreach (var menu in category.Menus)
                    category.MenuCollection.Add(menu);
                this.Categories.Add(category);
            }
        }
    }
}