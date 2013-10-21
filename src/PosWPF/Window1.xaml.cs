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
using System.Data.Linq;
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
using HiTea.Pos;

namespace PosWPF
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();
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

            //db.Category

            //var category = from c in db.Category select c;

            HiTea.Pos.Menu menu = new HiTea.Pos.Menu();
            menu.Code = "A03";
            menu.Name = "Coconut Pie";
            menu.Price = 33.3F;
            //db.GetTable(Category).Where
            
            //db.GetTable(Category).WHere
            //menu.Category = db.GetTable(Category).Where
            db.Menu.InsertOnSubmit(menu);
            db.SubmitChanges();
        }
    }
}