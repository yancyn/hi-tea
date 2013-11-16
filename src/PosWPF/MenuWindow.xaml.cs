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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        AdminManager manager;
        public MenuWindow(Main db)
        {
            InitializeComponent();

            manager = new AdminManager(db);
            this.DataContext = manager;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            manager.Commit();
        }
    }
}
