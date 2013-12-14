using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HiTea.Pos
{
    public partial class OrderItem
    {
        public ObservableCollection<OrderSubItem> SubItems { get; set; }
        partial void OnAmountChanged();
        partial void OnAmountChanging(float value);
        private float _amount;
        /// <summary>
        /// Return total amount of order item. Normally refer to menu price
        /// but some may have additional add on.
        /// </summary>
        public float Amount
        {
            get
            {
                CalculateAmount();
                return _amount;
            }
            set
            {
                if ((_amount != value))
                {
                    this.OnAmountChanging(value);
                    this.SendPropertyChanging();
                    this._amount = value;
                    this.SendPropertyChanged("Amount");
                    this.OnAmountChanged();
                }
            }
        }
        private void CalculateAmount()
        {
            _amount = 0f;
            if (this.Menu != null)
                _amount += this.Menu.Price;
            foreach (OrderSubItem sub in this.OrderSubItems)
                _amount += sub.Menu.Price;
        }

        public MarkDoneCommand MarkDoneCommand { get; set; }
        partial void OnCreated()
        {
            this.SubItems = new ObservableCollection<OrderSubItem>();
            this.SubItems.CollectionChanged += SubItems_CollectionChanged;
            this.MarkDoneCommand = new MarkDoneCommand(this);
        }

        void SubItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateAmount();
            this.SendPropertyChanged("Amount");
        }
        public void MarkDone()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db = new Main(connectionString);
            OrderItem orderItem = db.OrderItems.Where(i => i.ID == this.ID).First();
            orderItem.StatusID = this.StatusID;
            db.SubmitChanges();
            db.Dispose();
        }
    }

    public class MarkDoneCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            item.MarkDone();
        }
        private OrderItem item;
        public MarkDoneCommand(OrderItem item)
        {
            this.item = item;
        }
    }
}
