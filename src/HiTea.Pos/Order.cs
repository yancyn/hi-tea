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
        partial void OnCashChanged();
        partial void OnCashChanging(decimal value);
        private decimal cash;
        /// <summary>
        /// Cash paid by guest for consumption. Zero value will be represent not yet pay or paid by credit card.
        /// </summary>
        public decimal Cash
        {
            get { return this.cash; }
            set
            {
                if ((cash != value))
                {
                    this.OnCashChanging(value);
                    this.SendPropertyChanging();
                    this.cash = value;
                    this.SendPropertyChanged("Cash");
                    this.SendPropertyChanged("Return");
                    this.OnCashChanged();
                }
            }
        }

        partial void OnReturnChanged();
        partial void OnReturnChanging(decimal value);
        private decimal _return;
        /// <summary>
        /// Total need to be return after deduct from total amount.
        /// </summary>
        public decimal Return
        {
            get
            {
                _return = this.cash - (decimal)this._total;
                return _return;
            }
        }
        public ObservableCollection<OrderItem> Items { get; set; }
        partial void OnCreated()
        {
            this.cash = 0m;
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
