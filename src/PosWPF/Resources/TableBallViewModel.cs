using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using HiTea.Pos;

namespace PosWPF
{
    public partial class TableBallViewModel : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        partial void OnOrderChanged();
        partial void OnOrderChanging(Order value);

        private Order order;
        public Order Order
        {
            get { return this.order; }
            set
            {
                if (((order == value) == false))
                {
                    this.OnOrderChanging(value);
                    this.SendPropertyChanging();
                    this.order = value;
                    this.SendPropertyChanged("Order");
                    this.OnOrderChanged();
                }
            }
        }
        public System.Windows.Visibility Visibility { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public TableBallViewModel(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            this.Visibility = System.Windows.Visibility.Hidden;
        }
        public void SetOrder(Order order)
        {
            this.order = order;
            this.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
