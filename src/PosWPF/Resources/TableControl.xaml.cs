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
        /// <summary>
        /// Index of ball to display only.
        /// </summary>
        /// <remarks>
        /// Index in zero based.
        /// </remarks>
        public int[] DisplayIndexes
        {
            get { return (int[])GetValue(DisplayIndexesProperty); }
            set { SetValue(DisplayIndexesProperty, value); }
        }
        public static readonly DependencyProperty DisplayIndexesProperty = DependencyProperty.Register(
            "DisplayIndexes",
            typeof(int[]),
            typeof(TableControl));

        public string[] DisplayNames
        {
            get { return (string[])GetValue(DisplayNamesProperty); }
            set { SetValue(DisplayNamesProperty, value); }
        }
        public static readonly DependencyProperty DisplayNamesProperty = DependencyProperty.Register(
            "DisplayNames",
            typeof(string[]),
            typeof(TableControl));

        public TableControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PosManager pos = this.DataContext as PosManager;
            if (DisplayIndexes.Length <= pos.TableBasket.Count)
            {
                for (int i = 0; i < DisplayIndexes.Length; i++)
                {
                    int index = DisplayIndexes[i];
                    if (Grid.Children[index] is Button)
                    {
                        (Grid.Children[index] as Button).Visibility = System.Windows.Visibility.Visible;
                        (Grid.Children[index] as Button).DataContext = pos.TableBasket[i];
                    }
                }
            }
        }

        private void OpenOrder_Click(object sender, RoutedEventArgs e)
        {
            PosManager pos = this.DataContext as PosManager;
            pos.SelectedOrder = (sender as System.Windows.Controls.Button).DataContext as Order;
            if (String.IsNullOrEmpty(pos.SelectedOrder.QueueNo))
                pos.SelectedOrder.QueueNo = pos.GetLatestQueueNo();

            OrderWindow window = new OrderWindow();
            window.Owner = (sender as FrameworkElement).TemplatedParent as Window; // TODO: Hosted window but null
            window.Topmost = true;
            window.DataContext = this.DataContext;
            window.Show();
        }
    }
}
