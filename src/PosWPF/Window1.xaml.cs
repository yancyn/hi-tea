/*
 * Created by SharpDevelop.
 * User: BeanBean
 * Date: 10/17/2013
 * Time: 9:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;

using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using DbLinq.Data;
using DbLinq.Data.Linq;
using HiTea.Pos;

namespace PosWPF
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
        private PosManager posManager = new PosManager();
        private Timer currentTimeTimer = new Timer();
		public Window1()
		{
			InitializeComponent();

            //HiTea.Pos.User cashier = new HiTea.Pos.User();
            //cashier.Username = "yancyn";
            //posManager.Cashier = cashier;
            this.DataContext = posManager;

            currentTimeTimer.Interval = 1000 * 60;
            currentTimeTimer.Tick += timer_Tick;
            currentTimeTimer.Start();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow loginScreen = new LoginWindow();
            loginScreen.Owner = this;
            loginScreen.DataContext = this.DataContext;
            loginScreen.ShowDialog();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            posManager.CurrentTime.Now = DateTime.Now;
        }
		
		void button1_Click(object sender, RoutedEventArgs e)
		{
			string receipt = "This is a receipt\n";
			receipt += "\u73CD\n";
			receipt += "珍珠奶茶\t";
			System.Diagnostics.Debug.WriteLine(receipt);
			PrintFactory.SendTextToLPT1(receipt);
		}		
		void button2_Click(object sender, RoutedEventArgs e)
		{
			//PrintDocument pd = new PrintDocument();
			//pd.PrintPage += PrintPage;
			Print();
		}
//		private void PrintPage(object sender, PrintPageEventArgs e)
//		{
//			System.Drawing.Image img = System.Drawing.Image.FromFile("D:\\logo.png");
//			System.Drawing.Point loc = new System.Drawing.Point(100, 100);
//			e.Graphics.DrawImage(img, loc);
//		}
		public void Print()
        {
            System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();
            PrintDocument pdoc = new PrintDocument();
            
            PrinterSettings ps = new PrinterSettings();            
            PaperSize psize = new PaperSize("Custom", 400, 600);
            //ps.DefaultPageSettings.PaperSize = psize;
            pd.Document = pdoc;
            pd.Document.DefaultPageSettings.PaperSize = psize;
            //pdoc.DefaultPageSettings.PaperSize.Height =320;
            //pdoc.DefaultPageSettings.PaperSize.Height = 820;
            //pdoc.DefaultPageSettings.PaperSize.Width = 520;

            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintPage);
            System.Windows.Forms.DialogResult result = pd.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                PrintPreviewDialog pp = new PrintPreviewDialog();
                pp.Document = pdoc;
                result = pp.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    pdoc.Print();
                }
            }

        }
        void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("SimSun Regular", 10);// SimHei Regular
            float fontHeight = font.GetHeight();
            int startX = 10;//50;
            int startY = 10;//55;
            int Offset = 10;//40;
            graphics.DrawString("Welcome to MSST", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("Ticket No:", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("Ticket Date :" + "珍珠奶茶", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            String underLine = "------------------------------------------";
            graphics.DrawString(underLine, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db = new Main(connectionString);

            HiTea.Pos.Menu menu = new HiTea.Pos.Menu();
            menu.Code = "D" + new Random().Next(50);
            menu.Name = "Donut" + new Random().Next(50);
            menu.Price = new Random().Next(100) / 13F;
            Category category = db.Categories.Where(c => c.Name == "Food").FirstOrDefault();
            if (category != null) menu.CategoryID = category.ID;
            db.Menus.InsertOnSubmit(menu);
            db.SubmitChanges();
            System.Diagnostics.Debug.Write("Added a menu successfully.");
        }



        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void TakeAway_Click(object sender, RoutedEventArgs e)
        {
            posManager.TakeAway();
            posManager.SelectedOrder = posManager.CarryBasket[posManager.CarryBasket.Count - 1];

            OrderWindow window = new OrderWindow();
            window.Owner = this;
            window.Topmost = true;
            window.DataContext = posManager.SelectedOrder;
            window.Show();
        }
    }
}