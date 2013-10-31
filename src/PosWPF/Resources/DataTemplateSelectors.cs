using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using HiTea.Pos;

namespace PosWPF
{
    public class AdminTemplateSelector: DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //System.Diagnostics.Debug.WriteLine(item);
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null && item is AdminViewModel)
            {
                AdminViewModel viewModel = item as AdminViewModel;
                if (viewModel.Name == "User")
                    return element.FindResource("UserTemplate") as DataTemplate;
                else if (viewModel.Name == "Charge")
                    return element.FindResource("ChargeTemplate") as DataTemplate;
                else if (viewModel.Name == "Category")
                    return element.FindResource("CategoryTemplate") as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
