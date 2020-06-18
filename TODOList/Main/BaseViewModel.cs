using System;
using System.ComponentModel;

namespace TODOList
{
    [Serializable]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string v) // co to jest v? Staraj się używać pełnych nazw
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}
