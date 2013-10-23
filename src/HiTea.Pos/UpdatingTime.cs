using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HiTea.Pos
{
    public class UpdatingTime: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime now;
        public DateTime Now
        {
            get { return this.now; }
            set
            {
                this.now = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Now"));
                }
            }
        }

        public UpdatingTime()
        {
            this.now = DateTime.Now;
        }
    }
}
