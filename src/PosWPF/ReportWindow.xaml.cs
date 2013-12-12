using System;
using System.Collections.Generic;
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
            GenerateReport(DateTime.Now);
        }

        private void GenerateReport(DateTime from)
        {
            GenerateReport(from, new DateTime(from.Year, from.Month, from.Day, 23, 59, 59));
        }
        private void GenerateReport(DateTime from, DateTime to)
        {

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}