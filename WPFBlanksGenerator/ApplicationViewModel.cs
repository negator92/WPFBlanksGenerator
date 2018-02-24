﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;
using MessageBox = System.Windows.MessageBox;

namespace WPFBlanksGenerator
{
    public class ApplicationViewModel : PropertyChange
    {
        private const string OpenFileDialogFilter = "Project file|*.csproj";
        private const string OpenFileDialogTitle = "Select file csproj of WPF project";
        private string PropertyChange = @"using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;" +
$"\n\nnamespace {Project.Name}\n" +
@"{
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
}";

        public static Project Project { get; } = new Project();

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
                string file = Project.Path.Substring(0, Project.Path.Length - (Project.Name.Length + 7)) +
                              nameof(PropertyChange) + @".cs";
                OnPropertyChanged(Project.Name);
                OnPropertyChanged(PropertyChange);
                File.WriteAllText(file, PropertyChange);
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

                if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;

                if (!File.Exists(openFileDialog.FileName)) return;
                Project.Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                Project.Path = Path.GetFullPath(openFileDialog.FileName);
                XDocument xDocument = XDocument.Parse(File.ReadAllText(Project.Path));
                string frameworkVersion = xDocument.Descendants().SingleOrDefault(e => e.Name.LocalName == "TargetFrameworkVersion").Value;
                Project.Version = frameworkVersion;
            }
        }
    }
}