using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HiTea.Pos
{
    /// <summary>
    /// Pos manager class to manage main pos application.
    /// </summary>
    public partial class PosManager : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Maximum queue no to use. Once exceed the value will reset from 1 again.
        /// </summary>
        private int maxQueue;

        protected Main db;

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public Category Addon { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }

        /// <summary>
        /// Login user.
        /// </summary>
        public User CurrentUser { get; set; }
        /// <summary>
        /// Display current time.
        /// </summary>
        public UpdatingTime CurrentTime { get; set; }

        partial void OnSelectedOrderChanged();
        partial void OnSelectedOrderChanging(Order value);
        private Order selectedOrder;
        /// <summary>
        /// An order currently selected and working on.
        /// </summary>
        public Order SelectedOrder
        {
            get { return this.selectedOrder; }
            set
            {
                this.timer.Stop();
                if (((selectedOrder == value) == false))
                {
                    //this.OnSelectedOrderChanging(value);
                    //this.SendPropertyChanging();
                    this.selectedOrder = value;
                    //this.SendPropertyChanged("SelectedOrder");
                    //this.OnSelectedOrderChanged();
                }
            }
        }

        /// <summary>
        /// Represent all order on hand.
        /// </summary>
        public ObservableCollection<Order> Basket { get; set; }
        /// <summary>
        /// Subset of Basket which are take away order.
        /// </summary>
        public ObservableCollection<Order> CarryBasket { get; set; }

        private int tableSize;
        public int TableSize { get { return this.tableSize; } }
        /// <summary>
        /// Subject of basket which are dine in.
        /// </summary>
        public ObservableCollection<Order> TableBasket { get; set; }

        private LoginCommand loginCommand;
        public LoginCommand LoginCommand { get { return this.loginCommand; } }

        /// <summary>
        /// Timer event for refreshing order status from kitchen.
        /// </summary>
        System.Windows.Threading.DispatcherTimer timer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PosManager(Main db, int tableSize, int maxQueue)
        {
            this.tableSize = tableSize;
            this.maxQueue = maxQueue;

            this.loginCommand = new LoginCommand(this);
            this.dineInCommand = new DineInCommand(this);
            this.takeAwayCommand = new TakeAwayCommand(this);
            this.orderMenuCommand = new OrderMenuCommand(this);
            this.confirmOrderCommand = new ConfirmOrderCommand(this);

            this.CurrentUser = new User();
            this.CurrentTime = new UpdatingTime();
            this.Basket = new ObservableCollection<Order>();
            this.CarryBasket = new ObservableCollection<Order>();
            this.CarryBasket.CollectionChanged += CarryBasket_CollectionChanged;
            this.TableBasket = new ObservableCollection<Order>();
            this.TableBasket.CollectionChanged += TableBasket_CollectionChanged;

            // retrieve categoris and food menu
            this.db = db;
            this.Categories = new ObservableCollection<Category>();
            this.Menus = new ObservableCollection<Menu>();
            RefreshMenu();

            // bind all blank table first
            for (int i = TableSize-1; i >= 0; i--)
            {
                Order order = new Order();
                order.TableNo = (i + 1).ToString();
                this.TableBasket.Add(order);
            }

            // retrieve incomplete order
            var orders = db.Orders.Where(o => o.ReceiptDate.HasValue == false);
            foreach (Order order in orders)
            {
                if (order.MemberID > 0)
                    order.Member = db.Users.Where(u => u.ID == order.MemberID).First();

                foreach (OrderItem item in order.OrderItems)
                    order.Items.Add(item);
                //Order target = CloneOrder(order);
                if (String.IsNullOrEmpty(order.TableNo))
                    this.CarryBasket.Add(order);
                else
                {
                    int i = Convert.ToInt32(order.TableNo);
                    this.TableBasket[TableSize-i] = order;
                }
            }

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 10);
        }
        public void StartTimer()
        {
            this.timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // prevent updating a same database at a same time
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db2 = new Main(connectionString);
            foreach (Order order in this.Basket)
            {
                for (int i = 0; i < order.Items.Count; i++)
                {
                    Order latestOrder = db2.Orders.Where(o => o.ID == order.ID).First();
                    foreach (OrderItem item in latestOrder.OrderItems)
                    {
                        if (order.Items[i].ID == item.ID)
                        {
                            System.Diagnostics.Debug.WriteLine("Updating order status for " + order.Items[i].ID + ": " + item.StatusID);
                            order.Items[i].StatusID = item.StatusID;
                            break;
                        }
                    }
                }
            }
            db2.Dispose();
        }

        void TableBasket_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Order order in e.NewItems)
                    this.Basket.Add(order);
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Order order in e.OldItems)
                    this.Basket.Remove(order);
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                this.SendPropertyChanged("TableBasket");
            }
            System.Diagnostics.Debug.WriteLine("Basket: " + this.Basket.Count);
            System.Diagnostics.Debug.WriteLine("Table: " + this.TableBasket.Count);
        }
        void CarryBasket_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Order order in e.NewItems)
                    this.Basket.Add(order);
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Order order in e.OldItems)
                    this.Basket.Remove(order);
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                this.SendPropertyChanged("CarryBasket");
            }
            System.Diagnostics.Debug.WriteLine("Basket: " + this.Basket.Count);
            System.Diagnostics.Debug.WriteLine("Carry: " + this.CarryBasket.Count);
        }

        /// <summary>
        /// Refresh category with food menu collection after initialize or newly added.
        /// </summary>
        public void RefreshMenu()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db2 = new Main(connectionString);

            this.Categories.Clear();
            this.Menus.Clear();
            // TODO: Refresh DbLinq object
            int i = 0;
            foreach (var category in db2.Categories)
            {
                System.Diagnostics.Debug.WriteLine(category.Name);
                category.MenuCollection.Clear();
                if (i == 0)
                {
                    this.Addon = category;
                    this.Addon.MenuCollection.Clear();
                    foreach (var menu in category.Menus.Where(m => m.Active == true).OrderBy(m => m.Code))
                    {
                        System.Diagnostics.Debug.WriteLine("\t"+menu.Name);
                        this.Addon.MenuCollection.Add(menu);
                    }
                }
                else
                {
                    foreach (var menu in category.Menus.Where(m => m.Active == true).OrderBy(m => m.Code))
                    {
                        System.Diagnostics.Debug.WriteLine("\t" + menu.Name);
                        category.MenuCollection.Add(menu);
                        this.Menus.Add(menu);
                    }
                    this.Categories.Add(category);
                }

                i++;
            }
            db2.Dispose();

            this.SendPropertyChanged("Categories");
            this.SendPropertyChanged("Menus");
            this.SendPropertyChanged("Addon");
        }

        public bool Login(string username, string password)
        {
            User user = new User();
            user.Username = username;
            return user.Login(password);
        }
        public void SetUser(string username)
        {
            User user = db.Users.Where(u => u.Username == username || u.Email == username || u.Ic == username || u.Mobile == username).FirstOrDefault();
            if (user != null)
            {
                this.SelectedOrder.MemberID = user.ID;
                this.SelectedOrder.Member = user;
            }
        }

        /// <summary>
        /// Generate new queue no from database or holding cache.
        /// </summary>
        /// <returns></returns>
        public string GetLatestQueueNo()
        {
            int lastQueueNo = 0;

            Order lastOrder = db.Orders.OrderByDescending(o => o.ID).FirstOrDefault();
            if (lastOrder != null) lastQueueNo = Convert.ToInt32(lastOrder.QueueNo);
            if (this.Basket.Count > 0)
            {
                Order lastHold = this.Basket.OrderByDescending(b => b.QueueNo).First();
                lastQueueNo = Math.Max(lastQueueNo, Convert.ToInt32(lastHold.QueueNo));
            }
            lastQueueNo = lastQueueNo % maxQueue;

            lastQueueNo++;
            return lastQueueNo.ToString();
        }

        /// <summary>
        /// Add a new order into system.
        /// </summary>
        /// <param name="tableNo"></param>
        private void AddOrder(string tableNo)
        {
            Order order = new Order();
            order.TableNo = tableNo;
            order.QueueNo = GetLatestQueueNo();
            order.Created = DateTime.Now;

            if (order.TableNo.Length == 0)
                this.CarryBasket.Add(order);
            else
            {
                for(int i=0;i<this.TableBasket.Count;i++)
                {
                    if (this.TableBasket[i].TableNo == tableNo)
                    {
                        this.TableBasket[i] = order;
                        break;
                    }
                }
            }

            this.selectedOrder = order;
        }

        private DineInCommand dineInCommand;
        public DineInCommand DineInCommand { get { return this.dineInCommand; } }
        /// <summary>
        /// Add a new dine in order.
        /// </summary>
        /// <param name="tableNo"></param>
        public void DineIn(string tableNo)
        {
            AddOrder(tableNo);
        }       

        private TakeAwayCommand takeAwayCommand;
        public TakeAwayCommand TakeAwayCommand { get { return this.takeAwayCommand; } }
        /// <summary>
        /// Take away order.
        /// </summary>
        /// <param name="tableNo"></param>
        public void TakeAway()
        {
            AddOrder(string.Empty);
        }

        private OrderMenuCommand orderMenuCommand;
        public OrderMenuCommand OrderMenuCommand { get { return this.orderMenuCommand; } }
        /// <summary>
        /// Add a menu into selected order.
        /// </summary>
        /// <param name="menu"></param>
        public void OrderMenu(Menu menu)
        {
            if (this.selectedOrder == null) return;

            // HACK: Always first category is Addon
            if (menu.CategoryID == 1)
            {
                OrderItem item = this.selectedOrder.Items[this.selectedOrder.Items.Count - 1];

                OrderSubItem sub = new OrderSubItem();
                sub.ParentID = item.ID;
                sub.MenuID = menu.ID;
                sub.Menu = menu;
                item.OrderSubItems.Add(sub);
                item.SubItems.Add(sub);
            }
            else
            {
                OrderItem item = new OrderItem();
                item.Menu = menu;
                item.MenuID = menu.ID;
                item.ParentID = this.selectedOrder.ID;
                item.StatusID = 1;

                if (this.selectedOrder.Items.Count == 0)
                    item.OrderTypeID = (String.IsNullOrEmpty(this.selectedOrder.TableNo)) ? 2 : 1;
                else
                    item.OrderTypeID = this.selectedOrder.Items.Last().OrderTypeID;
                this.selectedOrder.Items.Add(item);
            }
        }

        private ConfirmOrderCommand confirmOrderCommand;
        public ConfirmOrderCommand ConfirmOrderCommand { get { return this.confirmOrderCommand; } }
        /// <summary>
        /// Confirm or update selected order into database.
        /// </summary>
        public void ConfirmOrder()
        {
            if (this.selectedOrder == null) return;
            System.Diagnostics.Debug.WriteLine("Updating order into database...");
            Order order = db.Orders.Where(o => o.ID == this.selectedOrder.ID).FirstOrDefault();
            if (order == null)
            {
                // IMPORTANT: Need to submit twice to get new ID. Header first then only children table.
                order = new Order();
                order.ID = this.selectedOrder.ID;
                order.Created = this.selectedOrder.Created;
                order.CreatedByID = this.selectedOrder.CreatedByID;
                order.DodAte = this.selectedOrder.DodAte;
                order.QueueNo = this.selectedOrder.QueueNo;
                order.ReceiptDate = this.selectedOrder.ReceiptDate;
                order.TableNo = this.selectedOrder.TableNo;
                order.MemberID = this.selectedOrder.MemberID;
                order.Total = this.selectedOrder.Total;
                db.Orders.InsertOnSubmit(order);
                System.Diagnostics.Debug.WriteLine("New ID: " + order.ID);

                order.OrderItems.Clear();
                foreach (OrderItem item in this.selectedOrder.Items)
                    order.OrderItems.Add(item);
                db.SubmitChanges();
                this.selectedOrder.ID = order.ID;
            }
            else
            {
                UpdateOrder(ref order);
            }

            this.selectedOrder = null;
            this.timer.Start();
        }
        public void UpdateOrder(ref Order order)
        {
            order.ID = this.selectedOrder.ID;
            order.Created = this.selectedOrder.Created;
            order.CreatedByID = this.selectedOrder.CreatedByID;
            order.DodAte = this.selectedOrder.DodAte;
            order.ReceiptDate = this.selectedOrder.ReceiptDate;
            order.QueueNo = this.selectedOrder.QueueNo;
            order.TableNo = this.selectedOrder.TableNo;
            order.MemberID = this.selectedOrder.MemberID;
            order.Total = this.selectedOrder.Total;

            List<OrderItem> oldItems = new List<OrderItem>();
            foreach (OrderItem item in order.OrderItems)
                oldItems.Add(item);

            order.OrderItems.Clear();
            foreach (OrderItem item in this.selectedOrder.Items)
                order.OrderItems.Add(item);

            // HACK: Manually DeleteOnSubmit since the code above fail in DbLinq
            foreach (OrderItem old in oldItems)
            {
                bool contains = false;
                foreach (OrderItem item in order.OrderItems)
                {
                    if (old.ID == item.ID)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains) db.OrderItems.DeleteOnSubmit(old);
            }

            db.SubmitChanges();
        }
        private Order CloneOrder(Order source)
        {
            Order order = new Order();
            order.ID = source.ID;
            order.Created = source.Created;
            order.CreatedByID = source.CreatedByID;
            order.DodAte = source.DodAte;
            order.QueueNo = source.QueueNo;
            order.ReceiptDate = source.ReceiptDate;
            order.MemberID = source.MemberID;
            order.TableNo = source.TableNo;
            order.Total = source.Total;

            order.Items = new ObservableCollection<OrderItem>();
            foreach (OrderItem item in source.OrderItems)
                order.Items.Add(CloneOrderItem(item));

            return order;
        }
        private OrderItem CloneOrderItem(OrderItem source)
        {
            OrderItem item = new OrderItem();
            item.ID = source.ID;
            item.ParentID = source.ParentID;
            item.OrderType = source.OrderType;
            item.OrderTypeID = source.OrderTypeID;
            item.MenuID = source.MenuID;
            item.Menu = source.Menu;
            item.StatusID = source.StatusID;
            return item;
        }
    }

    public class LoginCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            manager.Login(values[0].ToString(), values[1].ToString());
        }
        private PosManager manager;
        public LoginCommand(PosManager manager)
        {
            this.manager = manager;
        }
    }

    public class DineInCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.DineIn(parameter.ToString());
        }
        private PosManager manager;
        public DineInCommand(PosManager manager)
        {
            this.manager = manager;
        }
    }

    public class TakeAwayCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.TakeAway();
        }
        private PosManager manager;
        public TakeAwayCommand(PosManager manager)
        {
            this.manager = manager;
        }
    }

    public class OrderMenuCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.OrderMenu((Menu)parameter);
        }
        private PosManager manager;
        public OrderMenuCommand(PosManager manager)
        {
            this.manager = manager;
        }
    }

    public class ConfirmOrderCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        private PosManager manager;
        public ConfirmOrderCommand(PosManager manager)
        {
            this.manager = manager;
        }
        public void Execute(object parameter)
        {
            manager.ConfirmOrder();
        }
    }
}