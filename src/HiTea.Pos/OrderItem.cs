using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HiTea.Pos
{
    public partial class OrderItem
    {
        public MarkDoneCommand MarkDoneCommand { get; set; }
        partial void OnCreated()
        {
            this.MarkDoneCommand = new MarkDoneCommand(this);
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
