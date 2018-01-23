using System.Collections.Generic;
using System.IO;

namespace WPFBlanksGenerator
{
    public class Solution : PropertyChange
    {
        public string Name { get; set; } = "Solution name";
        public string Path { get; set; } = "Where place solution";
        public Project[] ProjectsArray { get; set; }
    }
}