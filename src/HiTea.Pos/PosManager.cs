﻿using System;
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
    public class PosManager
    {
        /// <summary>
        /// Maximum queue no to use. Once exceed the value will reset from 1 again.
        /// </summary>
        private const int MAX_QUEUE_NO = 100;
        protected Main db;
        public ObservableCollection<Category> Categories { get; set; }

        /// <summary>
        /// Login user.
        /// </summary>
        public User CurrentUser { get; set; }
        /// <summary>
        /// Display current time.
        /// </summary>
        public UpdatingTime CurrentTime { get; set; }
        /// <summary>
        /// An order currently selected and working on.
        /// </summary>
        public Order SelectedOrder { get; set; }

        /// <summary>
        /// Represent all order on hand.
        /// </summary>
        public ObservableCollection<Order> Basket { get; set; }
        /// <summary>
        /// Subject of basket which are dine in.
        /// </summary>
        public ObservableCollection<Order> TableBasket { get; set; }
        /// <summary>
        /// Subset of Basket which are take away order.
        /// </summary>
        public ObservableCollection<Order> CarryBasket { get; set; }

        private LoginCommand loginCommand;
        public LoginCommand LoginCommand { get { return this.loginCommand; } }

        public PosManager()
        {
            this.CurrentUser = new User();
            this.CurrentTime = new UpdatingTime();
            this.Basket = new ObservableCollection<Order>();
            this.CarryBasket = new ObservableCollection<Order>();
            this.CarryBasket.CollectionChanged += CarryBasket_CollectionChanged;
            this.TableBasket = new ObservableCollection<Order>();
            this.TableBasket.CollectionChanged += TableBasket_CollectionChanged;

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            db = new Main(connectionString);

            // retrieve categoris and food menu
            this.Categories = new ObservableCollection<Category>();
            foreach (var category in db.Categories)
            {
                foreach (var menu in category.Menus)
                    category.MenuCollection.Add(menu);
                this.Categories.Add(category);
            }

            this.loginCommand = new LoginCommand(this);
            this.takeAwayCommand = new TakeAwayCommand(this);
            this.orderMenuCommand = new OrderMenuCommand(this);
            this.confirmOrderCommand = new ConfirmOrderCommand(this);
        }

        void TableBasket_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Order order in e.OldItems)
                    this.Basket.Remove(order);
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Order order in e.NewItems)
                    this.Basket.Add(order);
            }
            System.Diagnostics.Debug.WriteLine("Basket: " + this.Basket.Count);
            System.Diagnostics.Debug.WriteLine("Table: " + this.TableBasket.Count);
        }
        void CarryBasket_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Order order in e.OldItems)
                    this.Basket.Remove(order);
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Order order in e.NewItems)
                    this.Basket.Add(order);
            }
            System.Diagnostics.Debug.WriteLine("Basket: " + this.Basket.Count);
            System.Diagnostics.Debug.WriteLine("Carry: " + this.CarryBasket.Count);
        }

        public bool Login(string username, string password)
        {
            User user = new User();
            user.Username = username;
            return user.Login(password);
        }

        /// <summary>
        /// Add a new order into system.
        /// </summary>
        /// <param name="tableNo"></param>
        private void AddOrder(string tableNo)
        {
            int lastQueueNo = 0;
            if (this.Basket.Count == 0)
            {
                Order lastOrder = db.Orders.OrderByDescending(o => o.ID).FirstOrDefault();
                if (lastOrder != null) lastQueueNo = Convert.ToInt32(lastOrder.QueueNo);
            }
            else
            {
                lastQueueNo = Convert.ToInt32(this.Basket.Last().QueueNo);
            }
            lastQueueNo++;

            Order order = new Order();
            order.TableNo = tableNo;
            order.QueueNo = lastQueueNo.ToString();
            order.Created = DateTime.Now;

            if (order.TableNo.Length == 0)
                this.CarryBasket.Add(order);
            else
                this.TableBasket.Add(order);
        }
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
            if (this.SelectedOrder == null) return;

            OrderItem item = new OrderItem();
            item.Menu = menu;
            item.MenuID = menu.ID;
            item.ParentID = this.SelectedOrder.ID;
            item.StatusID = 1;
            if (this.SelectedOrder.Items.Count == 0)
                item.OrderTypeID = (String.IsNullOrEmpty(this.SelectedOrder.TableNo)) ? 2 : 1;
            else
                item.OrderTypeID = this.SelectedOrder.Items.Last().OrderTypeID;
            this.SelectedOrder.Items.Add(item);
        }

        private ConfirmOrderCommand confirmOrderCommand;
        public ConfirmOrderCommand ConfirmOrderCommand { get { return this.confirmOrderCommand; } }
        /// <summary>
        /// Confirm or update selected order into database.
        /// </summary>
        public void ConfirmOrder()
        {
            if (this.SelectedOrder == null) return;
            System.Diagnostics.Debug.WriteLine("Updating order into database...");
            Order order = db.Orders.Where(o => o.ID == this.SelectedOrder.ID).FirstOrDefault();
            if (order == null)
            {
                // IMPORTANT: Need to submit twice to get new ID. Header first then only children table.
                order = new Order();
                order.ID = this.SelectedOrder.ID;
                order.Created = this.SelectedOrder.Created;
                order.CreatedByID = this.SelectedOrder.CreatedByID;
                order.DodAte = this.SelectedOrder.DodAte;
                order.OrderItems.Clear();
                order.QueueNo = this.SelectedOrder.QueueNo;
                order.ReceiptDate = this.SelectedOrder.ReceiptDate;
                order.TableNo = this.SelectedOrder.TableNo;
                order.Total = this.SelectedOrder.Total;
                db.Orders.InsertOnSubmit(order);
                System.Diagnostics.Debug.WriteLine("New ID: " + order.ID);

                foreach (OrderItem item in this.SelectedOrder.Items)
                    order.OrderItems.Add(item);
                db.SubmitChanges();
            }
            else
            {
                //CloneOrder(order);
                order.ID = this.SelectedOrder.ID;
                order.Created = this.SelectedOrder.Created;
                order.CreatedByID = this.SelectedOrder.CreatedByID;
                order.DodAte = this.SelectedOrder.DodAte;
                order.OrderItems.Clear();
                foreach (OrderItem item in this.SelectedOrder.Items)
                    order.OrderItems.Add(item);
                order.QueueNo = this.SelectedOrder.QueueNo;
                order.ReceiptDate = this.SelectedOrder.ReceiptDate;
                order.TableNo = this.SelectedOrder.TableNo;
                order.Total = this.SelectedOrder.Total;

                db.SubmitChanges();
            }
        }
        private void CloneOrder(Order order)
        {
            order.ID = this.SelectedOrder.ID;
            order.Created = this.SelectedOrder.Created;
            order.CreatedByID = this.SelectedOrder.CreatedByID;
            order.DodAte = this.SelectedOrder.DodAte;
            order.OrderItems.Clear();
            foreach (OrderItem item in this.SelectedOrder.Items)
                order.OrderItems.Add(item);
            order.QueueNo = this.SelectedOrder.QueueNo;
            order.ReceiptDate = this.SelectedOrder.ReceiptDate;
            order.TableNo = this.SelectedOrder.TableNo;
            order.Total = this.SelectedOrder.Total;
            //order.User
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