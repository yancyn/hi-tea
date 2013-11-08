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
    /// Interaction logic for Playground.xaml
    /// </summary>
    public partial class Playground : Window
    {
        public Playground()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db = new Main(connectionString);
            PosManager posManager = new PosManager(db);
            DineInGrid.DataContext = posManager;
        }
    }
}
