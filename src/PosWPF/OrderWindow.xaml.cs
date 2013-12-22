using System;
using System.Collections.Generic;
using System.Drawing;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HiTea.Pos;

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
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            PosManager posManager = (this.DataContext as PosManager);

            // remove queue no if failed to make any order
            if (posManager.SelectedOrder.Items.Count == 0)
            {
                if (posManager.CarryBasket.Contains(posManager.SelectedOrder))
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
                else
                {
                    for (int i = 0; i < posManager.TableBasket.Count; i++)
                    {
                        if (posManager.TableBasket[i].TableNo == posManager.SelectedOrder.TableNo)
                        {
                            Order newOrder = new Order();
                            newOrder.TableNo = posManager.SelectedOrder.TableNo;
                            newOrder.Created = DateTime.Now;
                            posManager.TableBasket[i] = newOrder;
                            posManager.SelectedOrder = posManager.TableBasket[i];

                            if (this.Tag is TableControl)
                                (this.Tag as TableControl).Binding(posManager);
                            break;
                        }
                    }
                }

                posManager.SelectedOrder.QueueNo = string.Empty;
            }

            // remove from cache once paid
            if (posManager.SelectedOrder.ReceiptDate.HasValue)
            {
                if (posManager.CarryBasket.Contains(posManager.SelectedOrder))
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
                else
                {
                    for (int i = 0; i < posManager.TableBasket.Count; i++)
                    {
                        if (posManager.TableBasket[i].TableNo == posManager.SelectedOrder.TableNo)
                        {
                            Order newOrder = new Order();
                            newOrder.TableNo = posManager.SelectedOrder.TableNo;
                            newOrder.Created = DateTime.Now;
                            posManager.TableBasket[i] = newOrder;
                            posManager.SelectedOrder = posManager.TableBasket[i];

                            // HACK: Need to rebind tablecontrol everytime
                            if (this.Tag is TableControl)
                                (this.Tag as TableControl).Binding(posManager);
                            break;
                        }
                    }
                }
            }

            posManager.StartTimer();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            PosManager posManager = (PosManager)this.DataContext;
            if (posManager.SelectedOrder.Items.Count == 0)
            {
                if (posManager.CarryBasket.Contains(posManager.SelectedOrder))
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
                posManager.SelectedOrder.QueueNo = string.Empty;
            }

            this.Close();
        }

        /// <summary>
        /// Print order list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderListButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Generate a text file ie. order.txt
            // 2. Execute a cmd
            // 3. Type receipt.txt > LPT1
            // 4. Done.
            Order order = (this.DataContext as PosManager).SelectedOrder;

            //using(System.IO.StreamWriter file = new System.IO.StreamWriter("order.txt", 
            //System.IO.StreamWriter writer = new System.IO.TextWriter(
            //System.IO.FileStream file = System.IO.File.Open("order.txt", System.IO.FileMode.Op
            string content = string.Empty;
            string line = "------------------------";
            content += line + "\n";
            content += "TIME: " + DateTime.Now.ToString(Settings.Default.DateTimeFormat) + "\n";
            content += line + "\n";
            content += "ORDER#" + order.QueueNo;
            if (order.TableNo.Length > 0) content += " " + "TABLE#" + order.TableNo;
            content += "\n";

            content += line + "\n";
            foreach (OrderItem item in order.Items)
            {
                // HACK: we don't care about order made before
                // HACK: Fail to send unicode to LPT1
                if (item.ID == 0)
                    content += item.Menu.Code + " " + ExtractEnglishName(item.Menu.Name) + "\n";
            }
            content += line + "\n";
            content += "\n\n\n\n\n\n\n\n";

            // always overriding if not exist create new
            System.IO.File.WriteAllText("order.txt", content);//, Encoding.UTF8);

            string baseDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //System.Diagnostics.Process.Start("cmd", "Type \""+baseDirectory+System.IO.Path.DirectorySeparatorChar+"receipt.txt\" LPT1");
            //System.Diagnostics.Process.Start("cmd", "D: & Type receipt.txt > LPT1");
            System.Diagnostics.Process.Start("cmd", "/C copy \"" + baseDirectory + System.IO.Path.DirectorySeparatorChar + "order.txt\" " + Settings.Default.OrderPrinter);
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            Numpad numpad = new Numpad();
            numpad.Owner = this;
            numpad.Topmost = true;
            numpad.DataContext = this.DataContext;
            numpad.ShowDialog();
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();
            PrintDocument pdoc = new PrintDocument();
            pd.Document = pdoc;
            pdoc.PrintPage += new PrintPageEventHandler(Receipt_PrintPage);
            pdoc.Print();
        }

        // TODO: Consider to convert to xaml layout for easier maintenance
        // http://www.bradcurtis.com/document-and-report-generation-using-xaml-wpf-data-binding-and-xps/
        private void Receipt_PrintPage(object sender, PrintPageEventArgs e)
        {
            string underline = "--------------------------------------------------";
            string tab = "    ";

            // TODO: Receipt print format
            Order order = (this.DataContext as PosManager).SelectedOrder;

            int startX = 10;
            int startY = 10;
            int offset = 10;
            Graphics graphics = e.Graphics;
            Font font = new Font("SimSun Regular", 10);
            Font bold = new Font("SimHei Bold", 10);
            float fontHeight = font.GetHeight();
            graphics.DrawImage(System.Drawing.Image.FromFile("logo.png"), startX+20, startY + offset);

            offset += 80;
            graphics.DrawString("Welcome to " + Settings.Default.CompanyName, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            // Print address
            offset += 20;
            graphics.DrawString("Tel: " + Settings.Default.Telephone, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString(Settings.Default.Address, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            offset += 20;
            offset += 20;
            string feed = string.Empty;
            feed += "#" + order.ID;
            feed += tab + "Table: " + order.TableNo;
            feed += tab + "Queue: " + order.QueueNo;
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString("Date: " + DateTime.Now.ToString(Settings.Default.DateTimeFormat), font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            //int i = 0;
            // TODO: Chop longer name which overlap the amount
            foreach (OrderItem item in order.Items)
            {
                //i++;
                offset += 20;

                // Break line for different encoding
                string code = string.Empty;
                code += item.Menu.Code + " ";

                // extract English character only
                string eng = ExtractEnglishName(item.Menu.Name);
                string other = (eng.Length == 0) ? item.Menu.Name : item.Menu.Name.Replace(eng, string.Empty);
                code += other.Trim();

                string price = item.Menu.Price.ToString(Settings.Default.MoneyFormat);
                graphics.DrawString(code.Trim(), font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);
                graphics.DrawString(price, font, new SolidBrush(System.Drawing.Color.Black), startX + 200, startY + offset);

                offset += 20;
                graphics.DrawString(eng.Trim(), font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

                // print additonal addon for drink & dessert
                foreach (OrderSubItem sub in item.OrderSubItems) //.SubItems)
                {
                    offset += 20;

                    // Break line for different encoding
                    code = string.Empty;
                    code += sub.Menu.Code + " ";

                    // extract English character only
                    eng = ExtractEnglishName(sub.Menu.Name);
                    other = (eng.Length == 0) ? sub.Menu.Name : sub.Menu.Name.Replace(eng, string.Empty);
                    code += other.Trim();

                    price = sub.Menu.Price.ToString(Settings.Default.MoneyFormat);
                    graphics.DrawString(eng.Trim(), font, new SolidBrush(System.Drawing.Color.Black), startX + 20, startY + offset);
                    graphics.DrawString(price, font, new SolidBrush(System.Drawing.Color.Black), startX + 200, startY + offset);
                }
            }

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            string connectionString = ConfigurationManager.ConnectionStrings["PosConnectionString"].ConnectionString;
            Main db2 = new Main(connectionString);
            List<Charge> charges = db2.Charges.Where(c => c.Active == true).OrderBy(c => c.Value).ToList();
            for (int c = 0; c < charges.Count; c++)
            {
                if (order.Charges[c] != 0)
                {
                    offset += 20;
                    feed = charges[c].Name;
                    graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);
                    graphics.DrawString(Math.Round(order.Charges[c], 2).ToString(Settings.Default.MoneyFormat), font, new SolidBrush(System.Drawing.Color.Black), startX + 200 - 10, startY + offset);
                }
            }
            db2.Dispose();

            offset += 20;
            feed = "Total ";
            string total = order.Total.ToString(Settings.Default.MoneyFormat);
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);
            graphics.DrawString(total, font, new SolidBrush(System.Drawing.Color.Black), startX + 200 - 10, startY + offset);

            offset += 20;
            feed = "Rounding ";
            string rounding = Utils.Rounding(Convert.ToDecimal(order.Total)).ToString(Settings.Default.MoneyFormat);
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);
            graphics.DrawString(rounding, bold, new SolidBrush(System.Drawing.Color.Black), startX + 200 - 10, startY + offset);

            offset += 20;
            feed = "Cash ";
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);
            graphics.DrawString(order.Cash.ToString(Settings.Default.MoneyFormat), font, new SolidBrush(System.Drawing.Color.Black), startX + 200 - 10, startY + offset);

            offset += 20;
            feed = "Return ";
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);
            graphics.DrawString(order.Return.ToString(Settings.Default.MoneyFormat), bold, new SolidBrush(System.Drawing.Color.Black), startX + 200 - 10, startY + offset);

            offset += 20;
            graphics.DrawString(underline, font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset);

            // Replace with facebook icon
            offset += 20;
            graphics.DrawImage(System.Drawing.Image.FromFile("facebook.png"), startX, startY + offset);
            graphics.DrawString(Settings.Default.Facebook, font, new SolidBrush(System.Drawing.Color.Black), startX + 20, startY + offset);

            // print business hour
            offset += 20;
            graphics.DrawString("Business Hour", font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);
            feed = string.Empty;
            if (Settings.Default.Start.DayOfWeek - Settings.Default.End.DayOfWeek != 0)
                feed = Settings.Default.Start.ToString("ddd") + " - " + Settings.Default.End.ToString("ddd");
            else
                feed = "Daily";
            feed += "    " + Settings.Default.Start.ToString("hh:mm tt") + " - " + Settings.Default.End.ToString("hh:mm tt");
            offset += 20;
            graphics.DrawString(feed, font, new SolidBrush(System.Drawing.Color.Black), startX + 20, startY + offset);

            offset += 20;
            graphics.DrawString("Thank you", font, new SolidBrush(System.Drawing.Color.Black), startX + 50, startY + offset);

            offset += 20;
            graphics.DrawString(" ", font, new SolidBrush(System.Drawing.Color.Black), startX, startY + offset); // HACK: Add a footer margin
        }

        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            Order order = (this.DataContext as PosManager).SelectedOrder;
            foreach (OrderItem item in order.Items)
            {
                if (item.ID > 0) continue; // not going to print before. We only care about new drink
                if (item.Menu.Category.Name.ToLower() == "drink" || item.Menu.Category.Name.ToLower() == "dessert")
                {
                    //System.Diagnostics.Debug.WriteLine("Printing label for " + item.Menu.Name);
                    System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
                    pd.PrintQueue = new PrintQueue(new PrintServer(), Settings.Default.LabelPrinter);

                    string line = "No: " + order.QueueNo;
                    if (!String.IsNullOrEmpty(order.TableNo)) line += "    " + "Table: " + order.TableNo;
                    line += "\n" + item.Menu.Name; // TODO: Truncate long text

                    string addon = string.Empty;
                    foreach(OrderSubItem sub in item.OrderSubItems)
                    {
                        string eng = ExtractEnglishName(sub.Menu.Name);// extract English character only
                        string other = (eng.Length == 0) ? sub.Menu.Name : sub.Menu.Name.Replace(eng, string.Empty);
                        addon += other + " ";
                    }
                    if(addon.Length > 0) line += "\n" + addon;

                    FlowDocument flowDocument = new FlowDocument();
                    flowDocument.FontSize = 10;
                    Paragraph paragraph = new Paragraph(new Run(line));
                    flowDocument.Blocks.Add(paragraph);
                    pd.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, "Drink Label");
                }
            }
        }

        private void MemberName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string username = (sender as System.Windows.Controls.TextBox).Text;
            (this.DataContext as PosManager).SetUser(username);
        }

        /// <summary>
        /// Extract English name only from given text.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ExtractEnglishName(string name)
        {
            string eng = string.Empty;
            Regex regex = new Regex("[a-zA-Z0-9 '()&-]");
            foreach (Match match in regex.Matches(name))
                eng += match.Value;
            return eng;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // pop up at center of left panel in parent window
            // TODO: Failed, owner always null
            if (this.Owner != null)
            {
                this.Left = Owner.Width / 2 - this.Width / 2;
                this.Height = Owner.Height / 2 - this.Height / 2;
            }
        }

        #region Failed
        // add content to first page
        /*Canvas canvas = new Canvas();

        TextBlock feed = new TextBlock();
        feed.FontSize = 12;
        feed.Foreground = System.Windows.Media.Brushes.Black;
        feed.Text = "No: " + order.QueueNo;
        Canvas.SetLeft(feed, 4);
        Canvas.SetTop(feed, 4);
        canvas.Children.Add(feed);

        feed = new TextBlock();
        feed.FontSize = 12;
        feed.Foreground = System.Windows.Media.Brushes.Black;
        feed.Text = item.Menu.Name;
        Canvas.SetLeft(feed, 4);
        Canvas.SetTop(feed, 14);
        canvas.Children.Add(feed);
        */
        // FAILED: pd.PrintVisual(canvas, "Drink Label");
        #endregion
    }
}