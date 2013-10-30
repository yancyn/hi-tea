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
    /// Admin manager class to create all basic food menu required.
    /// </summary>
    public class AdminManager
    {
        protected Main db;
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }
        public ObservableCollection<Charge> Charges { get; set; }
        public ObservableCollection<AdminViewModel> Options { get; set; }

        private CommitCommand commitCommand;
        public CommitCommand CommitCommand { get { return this.commitCommand; } }

        public AdminManager()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            db = new Main(connectionString);

            this.Options = new ObservableCollection<AdminViewModel>();
            this.Users = new ObservableCollection<User>();
            this.Charges = new ObservableCollection<Charge>();
            this.Categories = new ObservableCollection<Category>();
            this.Menus = new ObservableCollection<Menu>();

            // clone from database
            foreach (var user in db.Users)
                this.Users.Add(user);
            foreach (var menu in db.Menus)
                this.Menus.Add(menu);
            foreach (var charge in db.Charges)
                this.Charges.Add(charge);
            foreach (var category in db.Categories)
                this.Categories.Add(category);

            foreach (var category in this.Categories)
                this.Options.Add(new AdminViewModel(category.Name,category));
            this.Options.Add(new AdminViewModel("Charges", this.Charges));
            this.Options.Add(new AdminViewModel("User", this.Users));

            this.commitCommand = new CommitCommand(this);
        }
        public void Commit()
        {
            db.SubmitChanges();
        }
    }

    public class CommitCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            manager.Commit();
        }
        private AdminManager manager;
        public CommitCommand(AdminManager manager)
        {
            this.manager = manager;
        }
    }
}
