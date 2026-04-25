using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Application.DTOs.Email;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Application.Exceptions;
using FluentEmail.Core;
using System.IO;
using RazorLight;

namespace Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmailFactory _fluentEmail;
        public MailSettings _mailSettings { get; }
        public ILogger<EmailService> _logger { get; }
        //private readonly RazorLightEngine _razorEngine;

        public EmailService(IFluentEmailFactory fluentEmail, IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            _fluentEmail = fluentEmail;
           // _razorEngine = new RazorLightEngineBuilder()
           //.UseFileSystemProject(Directory.GetCurrentDirectory()) // Uses the project root as template location
           //.UseMemoryCachingProvider()
           //.Build();
        }


        //public async Task SendEmailAsync(EmailRequest request, string templatePath)
        //{
        //    // Load template and render it with the model
        //    //var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates/EmailTemplate.cshtml");
        //    string templateContent = await File.ReadAllTextAsync(templatePath);

        //    var model = new
        //    {
        //        FirstName = request.FirstName,
        //        LastName = request.LastName,
        //        Otp = request.Otp,
        //        Body = request.Body,
        //        Url = request.Url
        //    };

        //    string emailBody = await _razorEngine.CompileRenderStringAsync("EmailTemplate", templateContent, model);

        //    var email = new MimeMessage();
        //    email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom));
        //    email.To.Add(MailboxAddress.Parse(request.To));
        //    email.Subject = request.Subject;
        //    email.Body = new TextPart("html") { Text = emailBody };

        //    using var smtp = new SmtpClient();
        //    await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
        //    await smtp.AuthenticateAsync(_mailSettings.EmailFrom, _mailSettings.SmtpPass);
        //    await smtp.SendAsync(email);
        //    await smtp.DisconnectAsync(true);
        //}

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        //public async Task SendEmailAsync(EmailRequest mailRequest)
        //{
        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse(_mailSettings.EmailFrom);
        //    email.To.Add(MailboxAddress.Parse(mailRequest.To));
        //    email.Subject = mailRequest.Subject;
        //    var builder = new BodyBuilder();
        //    //if (mailRequest.Attachments != null)
        //    //{
        //    //    byte[] fileBytes;
        //    //    foreach (var file in mailRequest.Attachments)
        //    //    {
        //    //        if (file.Length > 0)
        //    //        {
        //    //            using (var ms = new MemoryStream())
        //    //            {
        //    //                file.CopyTo(ms);
        //    //                fileBytes = ms.ToArray();
        //    //            }
        //    //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
        //    //        }
        //    //    }
        //    //}
        //    //builder.HtmlBody = mailRequest.Body;
        //    //email.Body = builder.ToMessageBody();
        //    //using var smtp = new SmtpClient();
        //    //smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        //    //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //    //await smtp.SendAsync(email);
        //    //smtp.Disconnect(true);
        //}


        public async Task SendFluentEmailTemplate(EmailRequest request, string templatePath)
        {
            var fileDirectory = $"{Directory.GetCurrentDirectory()}/{templatePath}";
            var model = new
            {
                Otp = request.Otp,
                Body = request.Body,
                Subject = request.Subject,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Url = request.Url,
            };

            await _fluentEmail
                .Create()
                .To(request.To)
                .Subject(request.Subject)
                .UsingTemplateFromFile(fileDirectory, model)
                .SendAsync();
        }

        public async Task SendFeedBackFluentEmailTemplate(EmailRequest request, string templatePath)
        {

            var model = new
            {
                Otp = request.Otp,
                Body = request.Body,
                Subject = request.Subject,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Url = request.Url,
                FeedbackEmail = request.FeedBackEmail,
                FeedbackPhoneNumber = request.FeedBackPhoneNumber,
            };

            await _fluentEmail
                .Create()
                .To(request.To)
                .Subject(request.Subject)
                .CC(request.CopyContactUsEmailTo)
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/{templatePath}", model)
                .SendAsync();
        }

        public async Task SendDevAlertEmailTemplate(EmailRequest request, string templatePath)
        {

            var model = new
            {
                Body = request.Body,
            };

            await _fluentEmail
                .Create()
                .To(request.To)
                .Subject(request.Subject)
                .CC(request.DeveloperEmail)
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/{templatePath}", model)
                .SendAsync();
        }

        public async Task SendFluentReminderEmail(EmailRequest request, string templatePath)
        {

            var model = new
            {
                Otp = request.Otp,
                Body = request.Body,
                Subject = request.Subject,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Url = request.Url,
                Title = request.Title,
                ReminderTime = request.ReminderTime,
                ReminderDate = request.ReminderDate
            };

            await _fluentEmail
                .Create()
                .To(request.To)
                .Subject(request.Subject)
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/{templatePath}", model)
                .SendAsync();
        }

    }
}
