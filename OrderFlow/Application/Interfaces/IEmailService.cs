using Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
        Task SendFluentEmailTemplate(EmailRequest request, string templatePath);
        Task SendFeedBackFluentEmailTemplate(EmailRequest request, string templatePath);
        Task SendFluentReminderEmail(EmailRequest request, string templatePath);
        //Task SendEmailAsync(EmailRequest request, string templatePath);
    }
}
