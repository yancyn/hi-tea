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
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public TableControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TableManager manager = this.DataContext as TableManager;
            for (int i = 0; i < Grid.Children.Count; i++)
            {
                if (Grid.Children[i] is Button)
                    (Grid.Children[i] as Button).DataContext = manager.Balls[i];
            }
        }
    }
}
