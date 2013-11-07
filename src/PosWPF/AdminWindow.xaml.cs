using System;
using System.Collections.Generic;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private AdminManager adminManager = new AdminManager();

        public AdminWindow()
        {
            InitializeComponent();
            this.DataContext = adminManager;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            adminManager.Commit();
            (this.Owner.DataContext as PosManager).RefreshMenu();
        }
    }
}