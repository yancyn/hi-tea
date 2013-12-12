using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private Main db;
        public ReportWindow(Main db)
        {
            InitializeComponent();

            this.db = db;
            From.Text = DateTime.Now.ToString();
            GenerateReport(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        }

        private void GenerateReport(DateTime from)
        {
            GenerateReport(from, new DateTime(from.Year, from.Month, from.Day, 23, 59, 59));
        }
        private void GenerateReport(DateTime from, DateTime to)
        {
            // DbLinq failed to filter by date
            //var orders = db.Orders.Where(o => o.ReceiptDate.HasValue == true && o.Created.CompareTo(from) >= 0 && o.Created.CompareTo(to) <= 0);

            decimal total = 0m;
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            var result = db.Orders.Where(o => o.ReceiptDate.HasValue == true);
            foreach (Order order in result)
            {
                if (order.Created.CompareTo(from) >= 0 && order.Created.CompareTo(to) <= 0)
                {
                    System.Diagnostics.Debug.WriteLine(order.Total);
                    orders.Add(order);
                    total += Utils.Rounding(System.Convert.ToDecimal(order.Total));
                }
            }

            System.Diagnostics.Debug.WriteLine("Total count: " + orders.Count);
            Grid.DataContext = orders;
            Total.Text = Utils.Rounding(System.Convert.ToDecimal(total)).ToString(Settings.Default.MoneyFormat);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (To.SelectedDate.HasValue)
                GenerateReport(From.SelectedDate.Value, To.SelectedDate.Value);
            else
                GenerateReport(From.SelectedDate.Value, DateTime.Now);
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}