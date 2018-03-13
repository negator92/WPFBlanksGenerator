using System;
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

        public XDocument XDocumentSelected { get; set; }

        private string FodyWeaversSource
        {
            get => "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
"<Weavers>\n" +
"  <PropertyChanged />\n" +
"  <Costura />\n" +
"</Weavers>";
        }

        private string AppXamlSource
        {
            get => @"using System.Windows;" +
                   $"\n\nnamespace {Project.Name}\n" +
@"{
    public partial class App : Application
    {
        public ApplicationViewModel ApplicationViewModel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationViewModel = new ApplicationViewModel();
        }
    }
}";
        }

        private string RelayCommandSource
        {
            get => @"using System;
using System.Windows.Input;" +
                   $"\n\nnamespace {Project.Name}\n" +
                   @"{
    public class RelayCommand<T> : ICommand
    {
        protected Action<T> execute;
        protected Func<T, bool> canExecute;

        public RelayCommand(Action<T> execute) : this(execute, (Func<T, bool>)null) { }

        public RelayCommand(Action<T> execute, Func<bool> canExecute) : this(execute, t => canExecute()) { }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? (t => true);
        }

        public RelayCommand() { }

        public void Execute(object parameter) => execute((T)parameter);
        public bool CanExecute(object parameter) => canExecute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(t => execute()) { }
        public RelayCommand(Action<object> execute) : base(execute) { }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute) : base(execute, canExecute) { }
        public RelayCommand(Action execute, Func<bool> canExecute) : base(t => execute(), t => canExecute()) { }
        public RelayCommand(Action<object> execute, Func<bool> canExecute) : base(execute, canExecute) { }
    }
}";
        }

        private string PropertyChangeSource
        {
            get => @"using System;
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
        }

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
                string propertyChangeFile = Project.Path.Substring(0, Project.Path.Length - (Project.Name.Length + 7)) +
                                            nameof(PropertyChange) + @".cs";
                string relayCommandFile = Project.Path.Substring(0, Project.Path.Length - (Project.Name.Length + 7)) +
                                          nameof(RelayCommand) + @".cs";
                string appXamlFile = Project.Path.Substring(0, Project.Path.Length - (Project.Name.Length + 7)) +
                                     nameof(App) + @".xaml.cs";
                string fodyWeaversFile = Project.Path.Substring(0, Project.Path.Length - (Project.Name.Length + 7)) +
                                         @"FodyWeavers.xml";
                File.WriteAllText(propertyChangeFile, PropertyChangeSource);
                File.WriteAllText(relayCommandFile, RelayCommandSource);
                File.WriteAllText(appXamlFile, AppXamlSource);
                File.WriteAllText(fodyWeaversFile, FodyWeaversSource);
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
                XDocumentSelected = XDocument.Parse(File.ReadAllText(Project.Path));
                string frameworkVersion = XDocumentSelected.Descendants()
                    .SingleOrDefault(e => e.Name.LocalName == "TargetFrameworkVersion").Value;
                Project.Version = frameworkVersion;
            }
        }
    }
}