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

        public ObservableCollection<Order> Basket { get; set; }
        //public ObservableCollection<Order> CarryBasket { get; set; }

        private LoginCommand loginCommand;
        public LoginCommand LoginCommand {get {return this.loginCommand;}}
        
        public PosManager()
        {
            this.CurrentTime = new UpdatingTime();
            this.CurrentUser = new User();
            this.Basket = new ObservableCollection<Order>();

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
        }

        private bool isLoginSuccess = false;
        public bool IsLoginSuccess { }
        public bool Login(string username, string password)
        {
            User user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            isLoginSuccess = (user.Password == password) ? true : false;
            return isLoginSuccess;
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
}