using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Settings
{
    public class PeriodicLoginSettings
    {
        public string Email { get; set; }   
        public string Pass { get; set; }
        public string LogFilePath { get; set; }
    }
}
