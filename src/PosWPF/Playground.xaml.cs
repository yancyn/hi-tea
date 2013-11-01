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
    /// Interaction logic for Playground.xaml
    /// </summary>
    public partial class Playground : Window
    {
        public Playground()
        {
            InitializeComponent();

            Dictionary<int, string> displayBalls = new Dictionary<int, string>();
            displayBalls.Add(13, "A1");
            displayBalls.Add(36, "A2");
            displayBalls.Add(50, "A3");
            displayBalls.Add(80, "A4");

            TableManager manager = new TableManager(displayBalls);
            MainTable.DataContext = manager;
        }
    }
}
