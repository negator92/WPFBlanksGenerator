using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace WPFBlanksGenerator
{
    public class ApplicationViewModel : PropertyChange, INotifyPropertyChanged
    {
        public static Solution Solution { get; set; } = new Solution();

        public ICommand SetFolderCommand { get; set; }

        public ApplicationViewModel()
        {
            SetFolderCommand = new RelayCommand(SetFolder, () => true);
        }

        private void SetFolder()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    Solution.Path = dialog.SelectedPath;
            }
            MessageBox.Show($"You choose {Solution.Path}");
        }
    }
}