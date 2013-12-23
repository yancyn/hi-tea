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
        #region Properties
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
        #endregion

        public TableControl()
        {
            InitializeComponent();
            this.DataContextChanged += TableControl_DataContextChanged;
        }

        #region Events
        /// <summary>
        /// Always redraw table layout. Not really databinding.
        /// </summary>
        /// <param name="posManager"></param>
        public void Binding(PosManager posManager)
        {
            //System.Diagnostics.Debug.WriteLine("Binding TableControl");
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
            ResetDragDrop();

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
        #endregion

        #region Drag and drop
        // see http://wpftutorial.net/DragAndDrop.html
        private Point startPoint;
        private Button startButton = null;
        private void ResetDragDrop()
        {
            startPoint = new Point(0, 0);
            startButton = null;
        }
        private void button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("button_PreviewMouseLeftButtonDown");
            startPoint = e.GetPosition(null);
        }
        private void button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (startPoint == new Point(0, 0)) return;
            if (startButton != null) return;

            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                startButton = (sender as Button);
                if ((startButton.DataContext as TableBallViewModel).Order.Items.Count == 0)
                {
                    startButton = null;
                    return;
                }

                Button button = sender as Button;

                // find the data behind the listviewItem
                Order order = (button.DataContext as TableBallViewModel).Order;

                // initialize the drag and drop operation
                if (order != null && order.Items.Count > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("button_PreviewMouseMove");
                    DataObject dragData = new DataObject("Order", order);
                    DragDrop.DoDragDrop(button, dragData, DragDropEffects.Move);
                }
            }
        }
        private void button_DragEnter(object sender, DragEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("button_DragEnter");
            if (!e.Data.GetDataPresent("Order") || sender == e.Source)
                e.Effects = DragDropEffects.None;
        }
        private void button_Drop(object sender, DragEventArgs e)
        {
            if (startButton == null) return;

            if (e.Data.GetDataPresent("Order"))
            {
                PosManager posManager = this.DataContext as PosManager;
                Order order = e.Data.GetData("Order") as Order;
                if (order == null) return;
                if (order.Items.Count == 0) return;
                TableBallViewModel viewModel = (sender as Button).DataContext as TableBallViewModel;
                if (viewModel.Order.ID == order.ID) return; // skip if drag to itself

                //System.Diagnostics.Debug.WriteLine("button_Drop");
                string originalTableNo = order.TableNo;
                string destinationTableNo = ((sender as Button).DataContext as TableBallViewModel).Order.TableNo;
                for (int i = 0; i < posManager.TableBasket.Count; i++)
                {
                    // drag to destination table
                    if (posManager.TableBasket[i].TableNo == destinationTableNo)
                    {
                        posManager.TableBasket[i].Created = order.Created;
                        posManager.TableBasket[i].CreatedByID = order.CreatedByID;
                        posManager.TableBasket[i].DodAte = order.DodAte;
                        posManager.TableBasket[i].ID = order.ID;
                        posManager.TableBasket[i].Member = order.Member;
                        posManager.TableBasket[i].MemberID = order.MemberID;
                        posManager.TableBasket[i].OrderItems = order.OrderItems;
                        posManager.TableBasket[i].QueueNo = order.QueueNo;
                        posManager.TableBasket[i].Items.Clear();
                        foreach (OrderItem item in order.Items)
                            posManager.TableBasket[i].Items.Add(item);
                        posManager.TableBasket[i].ReceiptDate = order.ReceiptDate;
                        posManager.TableBasket[i].TableNo = destinationTableNo;
                        posManager.TableBasket[i].Total = order.Total;
                        //posManager.SelectedOrder = posManager.TableBasket[i];
                        //posManager.ConfirmOrder();
                    }

                    // empty original table
                    if (posManager.TableBasket[i].TableNo == originalTableNo)
                    {
                        posManager.TableBasket[i] = new Order();
                        posManager.TableBasket[i].TableNo = originalTableNo;
                    }
                }

                // HACK: Need to rebind tablecontrol everytime
                Binding(posManager);
            }

            ResetDragDrop();
        }
        #endregion
    }
}