using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Settings
{
    public class PayStackOptions
    {
        public string Key { get; set; }
        public string BaseUrl { get; set; }
        public string Callback_url { get; set; }
    }
}
