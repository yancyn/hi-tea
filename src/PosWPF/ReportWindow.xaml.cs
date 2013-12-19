using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private Main db;
        private ObservableCollection<Order> orders;

        public ReportWindow(Main db)
        {
            InitializeComponent();

            this.db = db;
            this.orders = new ObservableCollection<Order>();
            From.Text = DateTime.Now.ToString();
            GenerateReport(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        }

        private void GenerateReport(DateTime from)
        {
            GenerateReport(from, new DateTime(from.Year, from.Month, from.Day, 23, 59, 59));
        }
        private void GenerateReport(DateTime from, DateTime to)
        {
            // DbLinq failed to filter by date
            //var orders = db.Orders.Where(o => o.ReceiptDate.HasValue == true && o.Created.CompareTo(from) >= 0 && o.Created.CompareTo(to) <= 0);
            //decimal total = 0m;
            orders = new ObservableCollection<Order>();
            var result = db.Orders.Where(o => o.ReceiptDate.HasValue == true).OrderBy(o => o.ID);
            foreach (Order order in result)
            {
                if (order.Created.CompareTo(from) >= 0 && order.Created.CompareTo(to) <= 0)
                {
                    //System.Diagnostics.Debug.WriteLine(order.Total);
                    orders.Add(order);
                    //total += Utils.Rounding(System.Convert.ToDecimal(order.Total));
                }
            }

            //System.Diagnostics.Debug.WriteLine("Total count: " + orders.Count);
            Grid.DataContext = orders;
            Total.Text = orders.Sum(o => o.Total).ToString(Settings.Default.MoneyFormat);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add time filtering
            DateTime from = new DateTime(From.SelectedDate.Value.Year, From.SelectedDate.Value.Month, From.SelectedDate.Value.Day);
            DateTime to = (To.SelectedDate.HasValue)
                ? new DateTime(To.SelectedDate.Value.Year, To.SelectedDate.Value.Month, To.SelectedDate.Value.Day, 23, 59, 59) : DateTime.Now;
            GenerateReport(from, to);
        }
        /// <summary>
        /// Prefix table no with zero if less than 10.
        /// </summary>
        /// <param name="tableNo"></param>
        /// <returns></returns>
        private string FixTableNo(string tableNo)
        {
            string fix = tableNo;
            if (String.IsNullOrEmpty(tableNo))
                fix = "--";
            else if (tableNo.Length < 2)
                fix = "0" + tableNo;

            return fix;
        }
        /// <summary>
        /// Add prefix to a string to a fixed length.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="digit"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string AddPrefix(string text, int digit, string prefix)
        {
            string fix = text;
            if (string.IsNullOrEmpty(text))
            {
                fix = string.Empty;
                for (int i = 0; i < digit; i++)
                    fix += prefix;
            }
            else if (text.Length < digit)
            {
                fix = string.Empty;
                for (int i = digit - text.Length; i < digit; i++)
                    fix += prefix;
                fix += text;
            }

            return fix;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Generate a text file sales.txt
            // 2. Execute a cmd
            // 3. Type sales.txt > LPT1
            // 4. Done.

            string content = string.Empty;
            string line = "------------------------";
            content += line + "\n";
            content += Settings.Default.CompanyName + " Sales" + "\n";
            content += "Time: " + DateTime.Now.ToString(Settings.Default.DateTimeFormat) + "\n";
            content += orders.Count + " Orders" + "\n";
            content += line + "\n";
            content += "# | Table | Created | Amount" + "\n";
            content += line + "\n";
            foreach (Order order in orders)
                content += AddPrefix(order.QueueNo, Settings.Default.MaxQueue.ToString().Length, "0") + "  " + FixTableNo(order.TableNo) + " " + order.Created.ToString("hh:mm tt") + "  " + order.Total.ToString(Settings.Default.MoneyFormat) + "\n";
            content += line + "\n";
            content += "         TOTAL: " + orders.Sum(o => o.Total).ToString(Settings.Default.MoneyFormat) + "\n";
            content += line + "\n";
            content += "\n\n\n\n\n\n\n\n";

            // always overriding if not exist create new
            System.IO.File.WriteAllText("sales.txt", content); //, Encoding.UTF8);

            string baseDirectory = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //System.Diagnostics.Process.Start("cmd", "Type \""+baseDirectory+System.IO.Path.DirectorySeparatorChar+"receipt.txt\" LPT1");
            //System.Diagnostics.Process.Start("cmd", "D: & Type receipt.txt > LPT1");
            System.Diagnostics.Process.Start("cmd", "/C copy \"" + baseDirectory + System.IO.Path.DirectorySeparatorChar + "sales.txt\" " + Settings.Default.ReportPrinter);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string content = string.Empty;
            string delimiter = ",";
            //Created|Queue|Table|Type|Code|Menu|Amount
            content += "Created" + delimiter + "Queue" + delimiter + "Table" + delimiter + "Type" + delimiter + "Code" + delimiter + "Menu" + delimiter + "Amount";
            foreach (Order order in orders)
            {
                string tableNo = String.IsNullOrEmpty(order.TableNo) ? "0" : order.TableNo;
                string header = AddDoubleQuotes(order.Created.ToString("yyyy-MM-dd HH:mm:ss")) + delimiter + order.QueueNo + delimiter + tableNo;
                float realTotal = 0f;
                foreach (OrderItem item in order.OrderItems)
                {
                    string dineType = (item.OrderTypeID == 1) ? "IN" : "OUT";
                    content += "\n" + header + delimiter + dineType + delimiter + item.Menu.Code + delimiter + AddDoubleQuotes(item.Menu.Name) + delimiter + item.Menu.Price;
                    realTotal += item.Menu.Price;
                    foreach (OrderSubItem sub in item.OrderSubItems)
                    {
                        content += "\n" + header + delimiter + dineType + delimiter + sub.Menu.Code + delimiter + AddDoubleQuotes(sub.Menu.Name) + delimiter + sub.Menu.Price;
                        realTotal += sub.Menu.Price;
                    }
                }

                // add member discount for tally purpose
                if (order.MemberID > 0)
                {
                    User member = db.Users.Where(u => u.ID == order.MemberID).FirstOrDefault();
                    if (member != null)
                        content += "\n" + header + delimiter + string.Empty + delimiter + AddDoubleQuotes(member.Username) + delimiter + AddDoubleQuotes("Member: " + member.Ic) + delimiter + (order.Total - realTotal);
                }
            }

            // Prompt to SaveFileDialog
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "csv | *.csv";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                System.IO.File.WriteAllText(dialog.FileName, content, Encoding.UTF8);
        }
        private string AddDoubleQuotes(string text)
        {
            return "\"" + text + "\"";
        }
    }
}