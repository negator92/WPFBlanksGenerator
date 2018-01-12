using System.Collections.Generic;
using System.IO;

namespace WPFBlanksGenerator
{
    public class Solution
    {
        public string Name { get; set; } = "Solution name";
        public string Path { get; set; } = "Where place solution";
        public List<Project> ProjectsList { get; set; } = new List<Project>()
        {
            new Project(),
        };
    }
}