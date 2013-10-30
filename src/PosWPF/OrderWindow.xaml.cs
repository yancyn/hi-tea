using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HiTea.Pos;

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db = new Main(connectionString);

            Order order = new Order();

            OrderItem item = new OrderItem();
            item.MenuID = 1;
            item.Menu = db.Menus.Where(m => m.ID == 1).First();
            item.OrderTypeID = 1;
            item.OrderType = db.OrderTypes.Where(o => o.ID == 1).First();
            item.StatusID = 1;
            item.Status = db.Statuses.Where(s => s.ID == 1).First();
            order.Items.Add(item);

            item = new OrderItem();
            item.MenuID = 2;
            item.Menu = db.Menus.Where(m => m.ID == 2).First();
            item.OrderTypeID = 2;
            item.OrderType = db.OrderTypes.Where(o => o.ID == 2).First();
            item.StatusID = 2;
            item.Status = db.Statuses.Where(s => s.ID == 2).First();
            order.Items.Add(item);

            this.DataContext = order;
        }
    }
}
