/*
 * Created by SharpDevelop.
 * User: BeanBean
 * Date: 10/17/2013
 * Time: 9:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;

using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using DbLinq.Data;
using DbLinq.Data.Linq;
using HiTea.Pos;
using System.Windows.Markup;

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Main db;
        private PosManager posManager;
        private Timer currentTimeTimer = new Timer();
        public Window1()
        {
            InitializeComponent();

            currentTimeTimer.Interval = 1000 * 60;
            currentTimeTimer.Tick += timer_Tick;
            currentTimeTimer.Start();

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            db = new Main(connectionString);
            db.QueryCacheEnabled = false;
            posManager = new PosManager(db);

            this.DataContext = posManager;

            List<Int32> indexes = new List<int>();
            foreach (string i in Settings.Default.TableIndexes)
                indexes.Add(Convert.ToInt32(i));
            DineInGrid.DisplayIndexes = indexes.ToArray();
            DineInGrid.DataContext = posManager;

            //MenusControl.ItemsPanel = GetItemsPanelTemplate(0);
            //MenusControl.ItemsSource = posManager.Menus;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            posManager.CurrentTime.Now = DateTime.Now;
        }

        private void Login()
        {
            LoginWindow loginScreen = new LoginWindow();
            loginScreen.Owner = this;
            loginScreen.DataContext = this.DataContext;
            loginScreen.ShowDialog();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: db.SubmitChanges()?
            this.Close();
        }

        private void Category_Checked(object sender, RoutedEventArgs e)
        {
            MenusControl.ItemsSource = ((sender as FrameworkElement).DataContext as Category).MenuCollection;
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow(db);
            window.Owner = this;
            window.ShowDialog();
        }

        private void ShowOrderForm()
        {
            OrderWindow window = new OrderWindow();
            window.Owner = this;
            window.Topmost = true;
            window.DataContext = posManager;
            window.Show();
        }
        private void TakeAway_Click(object sender, RoutedEventArgs e)
        {
            posManager.TakeAway();
            ShowOrderForm();
        }
        private void OpenOrder_Click(object sender, RoutedEventArgs e)
        {
            posManager.SelectedOrder = (sender as System.Windows.Controls.Button).DataContext as Order;
            ShowOrderForm();
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            //MenusControl.ItemsPanel = GetItemsPanelTemplate(0);
            //MenusControl.ItemsSource = posManager.Menus;
        }
        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            //MenusControl.ItemsPanel = GetItemsPanelTemplate(1);
            //MenusControl.ItemsSource = posManager.Menus;
        }
        /// <summary>
        /// TODO: Failed to return ItemsPanelTemplate
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private ItemsPanelTemplate GetItemsPanelTemplate(int mode)
        {
            string xaml = string.Empty;
            switch (mode)
            {
                case 0:
                    xaml = @"<ItemsPanelTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                            <ItemsPanelTemplate>
                                <WrapPanel />
                        </ItemsPanelTemplate>
                    </ItemsPanelTemplate>";
                    break;
                case 1:
                    xaml = @"<ItemsPanelTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
                            <ItemsPanelTemplate>
                                <VirtualizingStackPanel Orientation='Vertical' />
                        </ItemsPanelTemplate>
                    </ItemsPanelTemplate>";
                    break;
            }
            return XamlReader.Parse(xaml) as ItemsPanelTemplate;
        }
    }
}