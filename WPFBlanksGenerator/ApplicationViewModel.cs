using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace WPFBlanksGenerator
{
    public class ApplicationViewModel : PropertyChange
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

                if (File.Exists(openFileDialog.FileName))
                {
                    try
                    {
                        Solution.Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                        Solution.Path = Path.GetFullPath(openFileDialog.FileName);
                        var v = File.ReadLines()
                        foreach (var project in )
                        {
                            
                        }
                        Solution.ProjectsArray = new Project[]
                        {
                            new Project(),
                        };
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"{e.Message}\n{e.StackTrace}", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}