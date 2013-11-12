using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media;
using HiTea.Pos;
using System.Windows;

namespace PosWPF
{
    /// <summary>
    /// Based on source color wheel calculation from base color #007acc see http://colorschemedesigner.com/#3v61T--Jgw0w0
    /// </summary>
    public class CategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] names = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            if (value is Category)
            {
                Category category = value as Category;
                if (category.Menus.Count == 0)
                    return names[category.ID - 1];
                else
                    return category.Menus[0].Code.Substring(0, 1);
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
                
                // TODO: Extract to static method
                // Break line for different encoding
                string name = string.Empty;
                name += menu.Code + " ";

                // extract English character only
                string eng = string.Empty;
                Regex regex = new Regex("[a-zA-Z0-9 '()&-]");
                foreach (Match match in regex.Matches(menu.Name))
                    eng += match.Value;
                string other = (eng.Length == 0) ? menu.Name: menu.Name.Replace(eng, string.Empty);
                name += other.Trim() + "\n" + eng.Trim();

                return name;
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
                return dateTime.Now.ToString("dd/MM/yyyy hh:mm tt"); // TODO: Global date format
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
    /// Display float or decimal value in 2 digits only.
    /// </summary>
    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float)
            {
                float money = (float)value;
                return money.ToString("###,##0.00");
            }
            if (value is decimal)
            {
                decimal money = (decimal)value;
                return money.ToString("###,##0.00");
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Rounding float or decimal value by smallest unit 5 cents.
    /// </summary>
    public class MoneyRoundingConverter : IValueConverter
    {
        /// <summary>
        /// Rounding method as smallest unit 5 cents.
        /// TODO: Move to helper class.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private decimal Rounding(decimal original)
        {
            decimal result = 0m;
            decimal rounded = Math.Round(original, 1);
            decimal half = rounded + 0.05m;

            decimal diff1 = rounded - original;
            if (diff1 < 0) diff1 = diff1 * -1;

            decimal diff2 = half - original;
            if (diff2 < 0) diff2 = diff2 * -1;

            result = (diff1 > diff2) ? half : rounded;
            return result;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float)
            {
                float money = (float)value;
                return Rounding(System.Convert.ToDecimal(money)).ToString("###,###,##0.00");
            }
            if (value is decimal)
            {
                decimal money = (decimal)value;
                return Rounding(money).ToString("###,###,##0.00");
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
            if (categoryName.Contains("set "))
            {
                return new SolidColorBrush(Color.FromRgb(0, 124, 204)); //#007ccc
            }
            else if (categoryName.Contains("food"))
            {
                return new SolidColorBrush(Color.FromRgb(0, 212, 110)); //#00d46e
            }
            else if (categoryName.Contains("小食") || categoryName.Contains("snack"))
            {
                return new SolidColorBrush(Color.FromRgb(255, 151, 115)); //#ff9773
            }
            else if (categoryName.Contains("beverage") || categoryName.Contains("drink"))
            {
                return new SolidColorBrush(Color.FromRgb(103, 180, 230)); //#67b4e6
            }
            else if (categoryName.Contains("dessert"))
            {
                return new SolidColorBrush(Color.FromRgb(255, 142, 0)); //#ff8e00
            }
            else if (categoryName.Contains("charge"))
            {
                return new SolidColorBrush(Color.FromRgb(255, 255, 0));
            }
            else if (categoryName == "user")
            {
                return new SolidColorBrush(Color.FromRgb(255, 65, 0)); //#ff4100
            }
            else
            {
                // TODO: Handle undefine color code
                return new SolidColorBrush(Color.FromRgb(255, 151, 115));
            }
        }
        private SolidColorBrush GetColorCode(int cat)
        {
            switch (cat)
            {
                case 1:
                    return new SolidColorBrush(Color.FromRgb(0, 124, 204)); //#007ccc
                case 2:
                    return new SolidColorBrush(Color.FromRgb(0, 212, 110)); //#00d46e
                case 3:
                    return new SolidColorBrush(Color.FromRgb(255, 151, 115)); //#ff9773
                case 4:
                    return new SolidColorBrush(Color.FromRgb(103, 180, 230)); //#67b4e6
                case 5:
                    return new SolidColorBrush(Color.FromRgb(255, 142, 0)); //#ff8e00
                default:
                    // TODO: Handle undefine color code
                    return new SolidColorBrush(Color.FromRgb(255, 151, 115));
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string categoryName = value.ToString().ToLower();
            object obj = value;
            if (obj is Menu)
            {
                Menu menu = (Menu)obj;
                return GetColorCode(menu.CategoryID);
            }

            if (obj is AdminViewModel)
            {
                AdminViewModel vm = (AdminViewModel)obj;
                obj = vm.Name.ToLower();
            }
            //System.Diagnostics.Debug.WriteLine(obj);

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

    /// <summary>
    /// Check the box if dine in. OrderTypeID = 1.
    /// </summary>
    public class OneIsTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                return ((int)value) == 1 ? true : false;
            }
            else if (value is Int32)
            {
                return (System.Convert.ToInt32(value) == 1) ? true : false;
            }
            

            throw new ArgumentException("Not supported type of " + value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Int32))
            {
                return ((bool)value == true) ? 1 : 2;
            }
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Check the box if take away. OrderTypeID = 2.
    /// </summary>
    public class TwoIsTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                return ((int)value) == 2 ? true : false;
            }
            else if (value is Int32)
            {
                return (System.Convert.ToInt32(value) == 2) ? true : false;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Int16))
            {
                return ((bool)value == true) ? 2 : 1;
            }
            else if (targetType == typeof(Int32))
            {
                return ((bool)value == true) ? 2 : 1;
            }

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Get index of collection.
    /// </summary>
    /// <see>http://blogs.microsoft.co.il/blogs/davids/archive/2010/04/17/how-to-bind-to-the-index-of-a-collection-in-wpf.aspx</see>
    public class IndexConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length > 1)
            {
                if (values[0] is OrderItem && values[1] is ObservableCollection<OrderItem>)
                {
                    OrderItem item = values[0] as OrderItem;
                    ObservableCollection<OrderItem> collection = values[1] as ObservableCollection<OrderItem>;
                    return (collection.IndexOf(item) + 1).ToString() + ". ";
                }
            }

            //throw new NotImplementedException();
            return "0. ";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }
        #endregion
    }

    /// <summary>
    /// If there is zero row count just hidden otherwise visible.
    /// TODO: Check Visibility.Collapsed
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
            }
            else if (value is Boolean)
            {
                return (Boolean)value ? Visibility.Visible : Visibility.Hidden;
            }
            else if (value is int)
            {
                return ((int)value) > 0 ? Visibility.Visible : Visibility.Hidden;
            }
            else if (value is Int32)
            {
                return System.Convert.ToInt32(value) > 0 ? Visibility.Visible : Visibility.Hidden;
            }
            else if (value is decimal)
            {
                return System.Convert.ToDecimal(value) > 0 ? Visibility.Visible : Visibility.Hidden;
            }
            else if (value is float)
            {
                float money = (float)value;
                return (money > 0) ? Visibility.Visible : Visibility.Hidden;
            }
            else if (value == null)
            {
                return Visibility.Visible;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
