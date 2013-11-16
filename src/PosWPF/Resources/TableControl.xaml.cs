using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<TableBallViewModel> balls;

        public TableControl()
        {
            InitializeComponent();
            this.DataContextChanged += TableControl_DataContextChanged;
        }

        /// <summary>
        /// HACK: Always redraw table layout. Not really databinding.
        /// </summary>
        /// <param name="posManager"></param>
        public void Binding(PosManager posManager)
        {
            this.balls = new ObservableCollection<TableBallViewModel>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    this.balls.Add(new TableBallViewModel(i, j));
            }
            if (DisplayIndexes.Length <= posManager.TableBasket.Count)
            {
                for (int i = 0; i < DisplayIndexes.Length; i++)
                {
                    int index = DisplayIndexes[i];
                    this.balls[index].SetOrder(posManager.TableBasket[i]);
                }
            }
            BallControl.ItemsSource = null;
            BallControl.ItemsSource = this.balls;
        }

        void TableControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PosManager posManager = (sender as FrameworkElement).DataContext as PosManager;
            Binding(posManager);
        }

        private void OpenOrder_Click(object sender, RoutedEventArgs e)
        {
            PosManager posManager = this.DataContext as PosManager;
            //posManager.SelectedOrder = (sender as System.Windows.Controls.Button).DataContext as Order;
            TableBallViewModel viewModel = (sender as System.Windows.Controls.Button).DataContext as TableBallViewModel;
            posManager.SelectedOrder = viewModel.Order;
            if (String.IsNullOrEmpty(posManager.SelectedOrder.QueueNo))
                posManager.SelectedOrder.QueueNo = posManager.GetLatestQueueNo();
            if (posManager.SelectedOrder.Created == DateTime.MinValue)
                posManager.SelectedOrder.Created = DateTime.Now;

            OrderWindow window = new OrderWindow();
            window.Tag = this; // HACK: For execute rebind
            window.Owner = (sender as FrameworkElement).TemplatedParent as Window; // TODO: Hosted window but null
            window.Topmost = true;
            window.DataContext = this.DataContext;
            window.Show();
        }
    }
}