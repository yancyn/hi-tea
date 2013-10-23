using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
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
}
