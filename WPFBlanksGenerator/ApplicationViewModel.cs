using System.ComponentModel;

namespace WPFBlanksGenerator
{
    public class ApplicationViewModel : PropertyChange, INotifyPropertyChanged
    {
        public static Solution Solution { get; set; } = new Solution();

        public ICommand SetFolder { get; set; }

        public ApplicationViewModel()
        {
            SetFolder = new RelayCommand();
        }
    }
}