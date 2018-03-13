using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFBlanksGenerator
{
    [Serializable]
    public class PropertyChange : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
                OnPropertyChanged(propertyName);
        }
    }
}