using System.Windows;

namespace WPFBlanksGenerator
{
    public partial class App : Application
    {
        public ApplicationViewModel ApplicationViewModel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationViewModel = new ApplicationViewModel();
        }
    }
}
