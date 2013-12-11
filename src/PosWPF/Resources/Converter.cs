using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using HiTea.Pos;

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
                return dateTime.Now.ToString(Settings.Default.DateTimeFormat);
            }
            if (value is DateTime)
            {

                DateTime dateTime = (DateTime)value;
                return dateTime.ToString(Settings.Default.DateTimeFormat);
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
                return money.ToString(Settings.Default.MoneyFormat);
            }
            if (value is decimal)
            {
                decimal money = (decimal)value;
                return money.ToString(Settings.Default.MoneyFormat);
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
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float)
            {
                float money = (float)value;
                return Utils.Rounding(System.Convert.ToDecimal(money)).ToString(Settings.Default.MoneyFormat);
            }
            if (value is decimal)
            {
                decimal money = (decimal)value;
                return Utils.Rounding(money).ToString(Settings.Default.MoneyFormat);
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
                return new SolidColorBrush(Color.FromRgb(255, 255, 0)); //#ffff00
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
                case 2:
                    return new SolidColorBrush(Color.FromRgb(0, 124, 204)); //#007ccc
                case 3:
                    return new SolidColorBrush(Color.FromRgb(0, 212, 110)); //#00d46e
                case 4:
                    return new SolidColorBrush(Color.FromRgb(255, 151, 115)); //#ff9773
                case 5:
                    return new SolidColorBrush(Color.FromRgb(103, 180, 230)); //#67b4e6
                case 6:
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

            throw new ArgumentException("Not supported type of " + value.GetType());
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
    /// Indicate pie angle.
    /// </summary>
    public class AngleConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ObservableCollection<OrderItem>)
            {
                return (double)360/(value as ObservableCollection<OrderItem>).Count;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Starting rotation degree based on index in a collection.
    /// </summary>
    public class RotationConverter : IMultiValueConverter
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
                    int index = collection.IndexOf(item);
                    int total = collection.Count;
                    return (double)index * 360 / total;
                }
            }

            return 0d;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }
        #endregion
    }

    /// <summary>
    /// Mark as green if the food is ready to surve otherwise red. Other type than food ignore.
    /// </summary>
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is OrderItem)
            {
                OrderItem item = (value as OrderItem);
                if (item.Menu.Category.Name.ToLower() == "drink" || item.Menu.Category.Name.ToLower() == "dessert")
                    return Brushes.Transparent; // Ignore
                else
                    return (item.StatusID == 1) ? Brushes.Red : Brushes.Green;
            }

            throw new ArgumentException("Not supported type of " + value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
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

    /// <summary>
    /// Disappear when enter in textbox.
    /// </summary>
    /// <remarks>
    /// For watermark use.
    /// Source http://stackoverflow.com/questions/833943/watermark-hint-text-textbox-in-wpf
    /// </remarks>
    public class TextInputToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Always test MultiValueConverter inputs for non-null
            // (to avoid crash bugs for views in the designer)
            if (values[0] is bool && values[1] is bool)
            {
                bool hasText = !(bool)values[0];
                bool hasFocus = (bool)values[1];

                if (hasFocus || hasText)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
