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
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Order order = (Order)this.DataContext;
            int value = Convert.ToInt32((sender as Button).Content);
            decimal old = decimal.Round(order.Cash, 2);
            order.Cash = old*10m + value / 100m;
            System.Diagnostics.Debug.WriteLine(order.Cash);
        }
        private void HundredButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (Order)this.DataContext;
            order.Cash = order.Cash * 100m;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (Order)this.DataContext;
            order.Cash = decimal.Round(order.Cash / 10m, 2);
        }
	}
}