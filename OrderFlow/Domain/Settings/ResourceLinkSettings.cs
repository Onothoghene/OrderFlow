using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Settings
{
    public class ResourceLinkSettings
    {
        public string AdvocateLink { get; set; }
        public string AdvocatePass { get; set; }
        public string AcademicLink { get; set; }
        public string AcademicPass { get; set; }
    }

    public class ResourceLinkDto
    {
        public string Label { get; set; }
        public string Link { get; set; }
        public string Pass { get; set; }
    }
}


