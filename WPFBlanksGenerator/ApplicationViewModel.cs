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
        private const string OpenFileDialogFilter = "Project file|*.csproj";
        private const string OpenFileDialogTitle = "Select file csproj of WPF project";

        public static Project Project { get; set; } = new Project();

        public ICommand SelectProjectCommand { get; set; }
        public ICommand UpdateProjectCommand { get; set; }

        public ApplicationViewModel()
        {
            SelectProjectCommand = new RelayCommand(SelectProject);
            UpdateProjectCommand = new RelayCommand(UpdateProject, () => Project != null);
        }

        private void UpdateProject()
        {
            try
            {
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.StackTrace}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SelectProject()
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
                    Project.Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    Project.Path = Path.GetFullPath(openFileDialog.FileName);
                    OnPropertyChanged()
                }
            }
        }

        private int GetProjectsCount()
        {
            int i = 0;
            foreach (string line in File.ReadLines(Project.Path))
                if (line.Contains("EndProject"))
                    i++;
            return i;
        }
    }
}