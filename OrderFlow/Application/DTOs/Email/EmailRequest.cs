using System;
using System.Collections.Generic;
using System.Text;


namespace Application.DTOs.Email
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string Url { get; set; }
        public List<string> Attachments { get; set; }
        
        public int Otp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FeedBackPhoneNumber { get; set; }
        public string FeedBackEmail { get; set; }
        public string CopyContactUsEmailTo { get; set; }

        public string DeveloperEmail { get; set; }

        public string Title { get; set; }
        public string ReminderTime { get; set; }
        public string ReminderDate { get; set; }
    }


}
