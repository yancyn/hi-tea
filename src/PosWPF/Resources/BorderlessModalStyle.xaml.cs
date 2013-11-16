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

namespace PosWPF
{
    /// <summary>
    /// Interaction logic for BorderlessModalStyle.xaml
    /// </summary>
    public partial class BorderlessModalStyle : ResourceDictionary
    {
        public BorderlessModalStyle()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = (sender as FrameworkElement).TemplatedParent as Window;
            window.Close();
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window window = (sender as FrameworkElement).TemplatedParent as Window;
            window.DragMove();
        }
    }
}