using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using HiTea.Pos;

namespace PosWPF
{
    /// <summary>
    /// Display menu name.
    /// </summary>
    public class MenuDisplayNameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is HiTea.Pos.Menu)
            {
                HiTea.Pos.Menu menu = (HiTea.Pos.Menu)value;
                return menu.Code + " " + menu.Name;
            }
            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convert date time to local format.
    /// </summary>
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is HiTea.Pos.UpdatingTime)
            {

                HiTea.Pos.UpdatingTime dateTime = (HiTea.Pos.UpdatingTime)value;
                return dateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            }
            if (value is DateTime)
            {

                DateTime dateTime = (DateTime)value;
                return dateTime.ToString("dd/MM/yyyy hh:mm tt");
            }
            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Based on source color wheel calculation from base color #007acc see http://colorschemedesigner.com/#3v61T--Jgw0w0
    /// </summary>
    public class MenuBackgroundConverter : IValueConverter
    {
        private SolidColorBrush GetColorCode(string categoryName)
        {
            categoryName = categoryName.ToLower();
            if (categoryName == "set meal")
            {
                return new SolidColorBrush(Color.FromRgb(0, 212, 99));
            }
            else if (categoryName == "food")
            {
                return new SolidColorBrush(Color.FromRgb(255, 190, 0));
            }
            else if (categoryName == "beverage" || categoryName == "drink")
            {
                return new SolidColorBrush(Color.FromRgb(0, 122, 204));
            }
            else if (categoryName == "dessert")
            {
                return new SolidColorBrush(Color.FromRgb(105, 111, 224));
            }
            else if (categoryName == "charges")
            {
                return new SolidColorBrush(Color.FromRgb(255, 255, 0));
            }
            else if (categoryName == "user")
            {
                return new SolidColorBrush(Color.FromRgb(255, 151, 115));
            }
            else
            {
                // TODO: Handle undefine color code
                return new SolidColorBrush(Color.FromRgb(255, 151, 115));
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string categoryName = value.ToString().ToLower();
            object obj = value;
            if (obj is AdminViewModel)
            {
                AdminViewModel vm = (AdminViewModel)obj;
                obj = vm.Name.ToLower();
            }
            System.Diagnostics.Debug.WriteLine(obj);

            if (obj is Category)
            {
                Category category = (Category)value;
                categoryName = category.Name.ToLower();
            }
            if (obj is Charge) categoryName = "charges";
            if (obj is User) categoryName = "user";

            return GetColorCode(categoryName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
