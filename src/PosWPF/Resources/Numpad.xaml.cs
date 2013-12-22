/*
 * Created by SharpDevelop.
 * User: User
 * Date: 10/31/2013
 * Time: 21:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HiTea.Pos;

namespace PosWPF
{
	/// <summary>
	/// Interaction logic for Numpad.xaml
	/// </summary>
	public partial class Numpad : Window
	{
		public Numpad()
		{
			InitializeComponent();
		}
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
            //if (order.Cash > 0m)
            //{
            //    order.ReceiptDate = DateTime.Now;
            //    foreach (OrderItem item in order.Items)
            //        item.StatusID = 2;
            //    (this.DataContext as PosManager).UpdateOrder(ref order);
            //}
            (this.DataContext as PosManager).SelectedOrder.ReceiptDate = DateTime.Now;
            if (order.Cash > 0m) (this.DataContext as PosManager).Pay(order.ID);
            //(this.DataContext as PosManager).SelectedOrder = null;
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
            int value = Convert.ToInt32((sender as Button).Content);
            decimal old = decimal.Round(order.Cash, 2);
            order.Cash = old*10m + value / 100m;
            //System.Diagnostics.Debug.WriteLine(order.Cash);
        }
        private void HundredButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
            order.Cash = order.Cash * 100m;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
            order.Cash = decimal.Round(order.Cash / 10m, 2);
        }
	}
}