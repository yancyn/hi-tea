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
        public LoginCommand LoginCommand {get {return this.loginCommand;}}
        
        public PosManager()
        {
            this.CurrentUser = new User();
            this.CurrentTime = new UpdatingTime();
            this.Basket = new ObservableCollection<Order>();
            this.CarryBasket = new ObservableCollection<Order>();
            this.TableBasket = new ObservableCollection<Order>();

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
        }

        private bool isLoginSuccess = false;
        public bool IsLoginSuccess { get { return this.isLoginSuccess; } }
        public bool Login(string username, string password)
        {
            User user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            isLoginSuccess = (user.Password == password) ? true : false;
            return isLoginSuccess;
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

            this.Basket.Add(order);
            if (order.TableNo.Length == 0)
                this.CarryBasket.Add(order);
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

        /// <summary>
        /// Add a menu into selected order.
        /// </summary>
        /// <param name="menu"></param>
        public void OrderMenu(Menu menu)
        {
            OrderItem item = new OrderItem();
            item.Menu = menu;
            item.ParentID = this.SelectedOrder.ID;
            item.StatusID = 1;
            this.SelectedOrder.Items.Add(item);
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
}