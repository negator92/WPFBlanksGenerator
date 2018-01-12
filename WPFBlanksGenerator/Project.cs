namespace WPFBlanksGenerator
{
    public class Project
    {
        public string Name { get; set; } = "Project name";
        public string NetFramevorkVersion { get; set; }
        public string[] NetFramevorkVersions { get; } = { "4.5", "4.5.1", "4.5.2", "4.6", "4.6.1", "4.6.2", "4.7" };
    }
}