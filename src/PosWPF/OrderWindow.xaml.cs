using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HiTea.Pos;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();

            //string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            //Main db = new Main(connectionString);

            //Order order = new Order();

            //OrderItem item = new OrderItem();
            //item.MenuID = 1;
            //item.Menu = db.Menus.Where(m => m.ID == 1).First();
            //item.OrderTypeID = 1;
            //item.OrderType = db.OrderTypes.Where(o => o.ID == 1).First();
            //item.StatusID = 1;
            //item.Status = db.Statuses.Where(s => s.ID == 1).First();
            //order.Items.Add(item);

            //item = new OrderItem();
            //item.MenuID = 2;
            //item.Menu = db.Menus.Where(m => m.ID == 2).First();
            //item.OrderTypeID = 2;
            //item.OrderType = db.OrderTypes.Where(o => o.ID == 2).First();
            //item.StatusID = 2;
            //item.Status = db.Statuses.Where(s => s.ID == 2).First();
            //order.Items.Add(item);

            //this.DataContext = order;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            PosManager posManager = (PosManager)this.DataContext;
            if (posManager.SelectedOrder.Items.Count == 0)
                posManager.CarryBasket.Remove(posManager.SelectedOrder);
            this.Close();
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();
            PrintDocument pdoc = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            PaperSize psize = new PaperSize("Custom", 400, 600); // TODO: Calculate the height based on total order items.
            pd.Document = pdoc;
            pd.Document.DefaultPageSettings.PaperSize = psize;

            pdoc.PrintPage += new PrintPageEventHandler(Receipt_PrintPage);
            PrintPreviewDialog pp = new PrintPreviewDialog();
            pp.Document = pdoc;
            pdoc.Print();
        }
        // TODO: Consider to convert to xaml layout for easier maintenance
        private void Receipt_PrintPage(object sender, PrintPageEventArgs e)
        {
            // TODO: Receipt print format
            Order order = (this.DataContext as PosManager).SelectedOrder;

            Graphics graphics = e.Graphics;
            Font font = new Font("SimSun Regular", 10);// TODO: SimHei Regular
            float fontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;
            int offset = 10;
            graphics.DrawString("Welcome to Hi Tea", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString("Queue No: " + order.QueueNo, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString("Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            String underline = "------------------------------------------";
            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            int i = 0;
            foreach (OrderItem item in order.Items)
            {
                i++;
                offset += 20;

                string line = i + ". " + item.Menu.Code + " " + item.Menu.Name;
                string price = item.Menu.Price.ToString("###,##0.00");
                graphics.DrawString(line, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);
                graphics.DrawString(price, font, new SolidBrush(System.Drawing.Color.Black), startX + 150, startY + offset);
            }

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            string feed = "Total: ";
            string total = order.Total.ToString("###,##0.00");
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);
            graphics.DrawString(total, font, new SolidBrush(System.Drawing.Color.Black), startX + 150, startY + offset);

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);
        }
    }
}