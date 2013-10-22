using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using HiTea.Pos;

namespace PosWPF
{
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
}
