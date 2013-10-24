using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    /// <summary>
    /// Pos manager class to manage main pos application.
    /// </summary>
    public class PosManager
    {
        protected Main db;
        public ObservableCollection<Category> Categories { get; set; }

        /// <summary>
        /// Login user.
        /// </summary>
        public User CurrentUser { get; set; }
        /// <summary>
        /// Display current time.
        /// </summary>
        public UpdatingTime CurrentTime { get; set; }
        /// <summary>
        /// An order currently selected and working on.
        /// </summary>
        public Order SelectedOrder { get; set; }

        /// <summary>
        /// Collection of orders dine in.
        /// </summary>
        public ObservableCollection<Order> TableBasket { get; set; }
        /// <summary>
        /// Collection of orders not dine in.
        /// </summary>
        public ObservableCollection<Order> CarryBasket { get; set; }

        
        public PosManager()
        {
            this.CurrentTime = new UpdatingTime();
            this.CurrentUser = new User();
            this.TableBasket = new ObservableCollection<Order>();
            this.CarryBasket = new ObservableCollection<Order>();

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            db = new Main(connectionString);

            // retrieve categoris and food menu
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