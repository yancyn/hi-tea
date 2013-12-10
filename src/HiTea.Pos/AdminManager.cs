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
        public ObservableCollection<AdminViewModel> Options { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Charge> Charges { get; set; }

        public AdminManager(Main db)
        {
            this.db = db;
            this.commitCommand = new CommitCommand(this);
            this.addMenuCommand = new AddMenuCommand(this);
            this.addUserCommand = new AddUserCommand(this);
            this.addChargesCommand = new AddChargesCommand(this);

            // clone from database
            this.Users = new ObservableCollection<User>();
            foreach (var user in db.Users)
                this.Users.Add(user);

            this.Charges = new ObservableCollection<Charge>();
            foreach (var charge in db.Charges)
                this.Charges.Add(charge);

            // retrieve categoris and food menu
            this.Categories = new ObservableCollection<Category>();
            foreach (var category in db.Categories)
            {
                category.MenuCollection.Clear();
                foreach (var menu in category.Menus.OrderBy(m => m.Code))
                    category.MenuCollection.Add(menu);
                this.Categories.Add(category);
            }

            this.Options = new ObservableCollection<AdminViewModel>();
            foreach (var category in this.Categories)
                this.Options.Add(new AdminViewModel(category.Name,category));
            this.Options.Add(new AdminViewModel("Charges", this.Charges));
            this.Options.Add(new AdminViewModel("User", this.Users));
        }

        private AddMenuCommand addMenuCommand;
        public AddMenuCommand AddMenuCommand { get { return this.addMenuCommand; } }
        public void AddMenu(int categoryId)
        {
            Category category = this.Categories.Where(c => c.ID == categoryId).First();
            Menu menu = new Menu();
            menu.Active = true;
            menu.CategoryID = categoryId;
            category.MenuCollection.Add(menu);
        }

        private AddUserCommand addUserCommand;
        public AddUserCommand AddUserCommand { get { return this.addUserCommand; } }
        public void AddUser()
        {
            User user = new User();
            user.RoleID = db.Roles.OrderByDescending(u => u.ID).First().ID;
            user.Active = true;
            this.Users.Add(user);
        }

        private AddChargesCommand addChargesCommand;
        public AddChargesCommand AddChargesCommand { get { return this.addChargesCommand; } }
        public void AddCharges()
        {
            Charge charge = new Charge();
            charge.Value = 0f;
            charge.Active = true;
            this.Charges.Add(charge);
        }

        private CommitCommand commitCommand;
        public CommitCommand CommitCommand { get { return this.commitCommand; } }
        public void Commit()
        {
            foreach (Category category in this.Categories)
            {
                foreach (Menu menu in category.MenuCollection)
                {
                    if (menu.ID == 0)
                        db.Menus.InsertOnSubmit(menu);
                }
            }

            foreach (User user in this.Users)
            {
                if (user.ID == 0)
                    db.Users.InsertOnSubmit(user);
            }
            foreach (Charge charge in this.Charges)
            {
                if(charge.ID == 0)
                    db.Charges.InsertOnSubmit(charge);
            }

            db.SubmitChanges();
            System.Diagnostics.Debug.WriteLine("Updating admin database...");
        }
    }

    public class AddMenuCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter is Category)
            {
                Category category = (Category)parameter;
                this.manager.AddMenu(category.ID);
            }
            else if (parameter is ObservableCollection<Menu>)
            {
                ObservableCollection<Menu> collection = (ObservableCollection<Menu>)parameter;
                Menu menu = collection.First();
                this.manager.AddMenu(menu.CategoryID);
            }
        }
        private AdminManager manager;
        public AddMenuCommand(AdminManager manager)
        {
            this.manager = manager;
        }
    }

    public class AddUserCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            this.manager.AddUser();
        }

        private AdminManager manager;
        public AddUserCommand(AdminManager manager)
        {
            this.manager = manager;
        }
    }

    public class AddChargesCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            this.manager.AddCharges();
        }

        private AdminManager manager;
        public AddChargesCommand(AdminManager manager)
        {
            this.manager = manager;
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
