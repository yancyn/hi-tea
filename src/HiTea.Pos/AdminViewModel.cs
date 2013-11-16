using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class AdminViewModel: INotifyPropertyChanged
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public AdminViewModel(string name, object value)
        {
            this.Value = value;
            this.Name = name;
            this.PropertyChanged += AdminViewModel_PropertyChanged;
        }

        void AdminViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}