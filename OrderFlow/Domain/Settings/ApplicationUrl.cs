using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Settings
{
    public class ApplicationUrl
    {
        public string HostedApiUrl { get; set; }
        public string PlatformTourUrl { get; set; }
        public string LoginUrl { get; set; }
        public string ConfirmEmailUrl { get; set; }
        public string AcceptToJoinEmail { get; set; }
        public string LoggerPath { get; set; }
        public string ForgetPasswordUrl { get; set; }
        public string InviteUrl { get; set; }
        public string SecretariatLoginUrl { get; set; }
    }
}
