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
                else if (viewModel.Name.ToLower().Contains("charge"))
                    return element.FindResource("ChargeTemplate") as DataTemplate;
                else if (viewModel.Name.ToLower().Contains("set"))
                    return element.FindResource("SetMenuTemplate") as DataTemplate;
                else if (viewModel.Name.ToLower().Contains("food"))
                    return element.FindResource("FoodMenuTemplate") as DataTemplate;
                else if (viewModel.Name.ToLower().Contains("drink"))
                    return element.FindResource("DrinkMenuTemplate") as DataTemplate;
                else if (viewModel.Name.ToLower().Contains("dessert"))
                    return element.FindResource("DessertMenuTemplate") as DataTemplate;
                else if (viewModel.Name.ToLower().Contains("小食") || viewModel.Name.ToLower().Contains("snack"))
                    return element.FindResource("SnackMenuTemplate") as DataTemplate;
                else if (viewModel.Name.ToLower().Contains("addon"))
                    return element.FindResource("AddonMenuTemplate") as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
