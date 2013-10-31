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
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : ResourceDictionary
    {
        public ModalWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = (sender as FrameworkElement).TemplatedParent as Window;
            if (window.DataContext is PosManager)
            {
                PosManager posManager = (PosManager)window.DataContext;
                if (posManager.SelectedOrder.Items.Count == 0)
                    posManager.CarryBasket.Remove(posManager.SelectedOrder);
            }

            window.Close();
        }
    }
}
