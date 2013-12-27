using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

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
                _return = this.cash - Utils.Rounding(Convert.ToDecimal(this.Total));
                return _return;
            }
        }
        public ObservableCollection<OrderItem> Items { get; set; }

        partial void OnCreated()
        {
            this.RemoveItemCommand = new RemoveItemCommand(this);
            this.cash = 0m;

            this.chargesPercentage = new Dictionary<string, float>();
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db2 = new Main(connectionString);
            try
            {
                foreach (Charge charge in db2.Charges.Where(c => c.Active == true))
                    this.chargesPercentage.Add(charge.Name, charge.Value);
            }
            finally { db2.Dispose(); }

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
            float amount = 0;
            foreach (OrderItem item in this.Items)
            {
                foreach (OrderSubItem sub in item.OrderSubItems)
                    amount += sub.Menu.Price;
                amount += item.Menu.Price;
            }
            this._total = amount;

            this.Charges = new ObservableCollection<float>();
            foreach (KeyValuePair<string, float> charge in this.chargesPercentage.OrderBy(c => c.Value))
            {
                float tax = 0f;
                if (charge.Value < 0)
                {
                    if (this.MemberID > 0)
                        tax = this._total * charge.Value;
                }
                else
                    tax = this._total * charge.Value;
                this.Charges.Add(tax);
                this._total += tax;
            }

            this._total = (float)Utils.Rounding(Convert.ToDecimal(this._total));
        }

        private Dictionary<string, float> chargesPercentage;
        public ObservableCollection<float> Charges { get; set; }
        [Column(Storage = "_total", Name = "Total", DbType = "real", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public float Total
        {
            get
            {
                if (this.Items.Count > 0)
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

        public User Member { get; set; }
        [Column(Storage = "_memberID", Name = "MemberId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int MemberID
        {
            get
            {
                return this._memberID;
            }
            set
            {
                if ((_memberID != value))
                {
                    this.OnMemberIDChanging(value);
                    this.SendPropertyChanging();
                    this._memberID = value;
                    this.SendPropertyChanged("MemberID");
                    this.OnMemberIDChanged();

                    CalculateTotal();
                    this.SendPropertyChanged("Total");
                }
            }
        }

        public RemoveItemCommand RemoveItemCommand { get; set; }
        /// <summary>
        /// Remove selected order item. Void the item from order menu.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(OrderItem item)
        {
            this.Items.Remove(item);
        }

        // Not use at this moment
        /// <summary>
        /// True if all food has been served. Not include drink and dessert.
        /// </summary>
        /// <returns></returns>
        public bool IsServed()
        {
            bool served = true;
            foreach (OrderItem item in this.Items)
            {
                if (item.StatusID != 2)
                    return false;
            }

            return served;
        }
    }

    public class RemoveItemCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter is OrderItem)
                this.order.RemoveItem(parameter as OrderItem);
        }
        private Order order;
        public RemoveItemCommand(Order order)
        {
            this.order = order;
        }
    }
}