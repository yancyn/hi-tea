using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public partial class Order
    {
        public ObservableCollection<OrderItem> Items { get; set; }
        partial void OnCreated()
        {
            this.Items = new ObservableCollection<OrderItem>();
        }
    }
}
