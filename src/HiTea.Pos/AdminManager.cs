using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    /// <summary>
    /// Admin manager class to create all basic food menu required.
    /// </summary>
    public class AdminManager
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Menu> Menus { get; set; }
        public ObservableCollection<Charge> Charges { get; set; }

        public AdminManager()
        {
        }
    }
}
