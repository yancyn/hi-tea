using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public partial class Category
    {
        public ObservableCollection<Menu> MenuCollection { get; set; }
        partial void OnCreated()
        {
            this.MenuCollection = new ObservableCollection<Menu>();
        }
    }
}
