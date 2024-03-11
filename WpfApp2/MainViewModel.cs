using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ToolVM
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                IsTopMost = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private bool _isTopMost;
        public bool IsTopMost
        {
            get => _isTopMost;
            set
            {
                _isTopMost = value;
                OnPropertyChanged("IsTopMost");
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
