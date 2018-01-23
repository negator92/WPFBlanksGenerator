using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace WPFBlanksGenerator
{
    public class ApplicationViewModel : PropertyChange, INotifyPropertyChanged
    {
        private const string OpenFileDialogFilter = "Solution file|*.sln";
        private const string OpenFileDialogTitle = "Select a Solution file with WPF project";

        public static Solution Solution { get; set; } = new Solution();

        public ICommand SetSlnCommand { get; set; }

        public ApplicationViewModel()
        {
            SetSlnCommand = new RelayCommand(SetFolder, () => true);
        }

        private void SetFolder()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.DefaultExt = OpenFileDialogFilter.Substring(14, 4);
                openFileDialog.Filter = OpenFileDialogFilter;
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Multiselect = false;
                openFileDialog.Title = OpenFileDialogTitle;

                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;
                
                try
                {
                    if (File.Exists(openFileDialog.FileName))
                        Solution.Path = openFileDialog.FileName;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}\n{e.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MessageBox.Show($"You choose {Solution.Path}");
        }
    }
}