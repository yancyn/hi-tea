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

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private PosManager posManager = new PosManager();
        private Timer currentTimeTimer = new Timer();
        public Window1()
        {
            InitializeComponent();

            currentTimeTimer.Interval = 1000 * 60;
            currentTimeTimer.Tick += timer_Tick;
            currentTimeTimer.Start();

            this.DataContext = posManager;

            Order order = new Order();
            order.TableNo = "1";
            posManager.TableBasket.Add(order);

            order = new Order();
            order.TableNo = "2";
            posManager.TableBasket.Add(order);

            order = new Order();
            order.TableNo = "3";
            posManager.TableBasket.Add(order);

            order = new Order();
            order.TableNo = "4";
            posManager.TableBasket.Add(order);

            order = new Order();
            order.TableNo = "5";
            posManager.TableBasket.Add(order);
            DineInGrid.DataContext = posManager;
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
            this.Close();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow();
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
    }
}