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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2: Window
    {
        public Window2()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db = new Main(connectionString);
            db.QueryCacheEnabled = false;

            PosManager posManager = new PosManager(db, 13, 100);
            button1.DataContext = posManager.TableBasket[11];
        }

        private Point startPoint;
        private Button startButton;
        private void button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button_PreviewMouseLeftButtonDown");
            startPoint = e.GetPosition(null);
        }
        private void button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button_MouseLeftButtonDown");
            startPoint = e.GetPosition(null);
        }
        private void button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (startPoint == new Point(0, 0)) return;

            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                System.Diagnostics.Debug.WriteLine("button_PreviewMouseMove");
                startButton = (sender as Button);

                Button button = sender as Button;

                // find the data behind the listviewItem
                Order order = button.DataContext as Order;

                // initialize the drag and drop operation
                if (order != null)
                {
                    DataObject dragData = new DataObject("Order", order);
                    DragDrop.DoDragDrop(button, dragData, DragDropEffects.Move);
                }
            }
        }
        private void button_DragEnter(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button_DragEnter");
            if (!e.Data.GetDataPresent("Order") || sender == e.Source)
                e.Effects = DragDropEffects.None;
        }
        private void button_Drop(object sender, DragEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button_Drop");
            if (e.Data.GetDataPresent("Order"))
            {
                Order order = e.Data.GetData("Order") as Order;
                (sender as Button).DataContext = order;
                if (startButton != null) startButton.DataContext = null;

                startPoint = new Point(0, 0);
                startButton = null;
            }
        }
    }
}