using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;
using MessageBox = System.Windows.MessageBox;
using System.Xml.XPath;
using System.Xml;

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
                    XDocument xDocument = XDocument.Parse(File.ReadAllText(Project.Path));
                    // XNamespace xNamespace = xDocument.Root.GetDefaultNamespace();
                    // XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
                    // xmlNamespaceManager.AddNamespace("", xNamespace.NamespaceName);
                    XNode xRoot = xDocument.Root;
                    // string targetFramevorkVersionNode = xDocument.XPathSelectElement("TargetFrameworkVersion", xmlNamespaceManager).Value;
                    string targetFramevorkVersionNode = xDocument.XPathSelectElement("TargetFrameworkVersion").Value;
                    Project.Version = targetFramevorkVersionNode.ToString();
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