using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DbLinq.Data.Linq;
using DbLinq.Vendor;

namespace HiTea.Pos
{
    public partial class Order
    {
        public ObservableCollection<OrderItem> Items { get; set; }
        partial void OnCreated()
        {
            this.Items = new ObservableCollection<OrderItem>();
            this.Items.CollectionChanged += Items_CollectionChanged;
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateTotal();
            this.SendPropertyChanged("Total");
        }
        private void CalculateTotal()
        {
            this._total = 0;
            foreach (OrderItem item in this.Items)
                this._total += item.Menu.Price;
        }

        [Column(Storage = "_total", Name = "Total", DbType = "real", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public float Total
        {
            get
            {
                CalculateTotal();
                return this._total;
            }
            set
            {
                if ((_total != value))
                {
                    this.OnTotalChanging(value);
                    this.SendPropertyChanging();
                    this._total = value;
                    this.SendPropertyChanged("Total");
                    this.OnTotalChanged();
                }
            }
        }
    }
}
