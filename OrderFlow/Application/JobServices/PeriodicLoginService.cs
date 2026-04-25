using Application.DTOs.Email;
using Application.Helper;
using Application.Interfaces;
using Domain.Settings;
using Microsoft.Extensions.Options;
using System;

namespace Application.JobServices
{
    public class PeriodicLoginService : IPeriodicLoginService
    {
        private readonly IAccountService _account;
        private readonly ILogWriter _logger;
        private readonly IEmailService _emailService;
        private readonly MailSettings _mailSettings;
        private readonly PeriodicLoginSettings _periodicSetting;

        public PeriodicLoginService(IAccountService account, ILogWriter logger, IEmailService emailService,
            IOptionsSnapshot<MailSettings> mailSettings, IOptionsSnapshot<PeriodicLoginSettings> periodicSetting)
        {
            _account = account;
            _logger = logger;
            _emailService = emailService;
            _mailSettings = mailSettings.Value;
            _periodicSetting = periodicSetting.Value;
        }

        public void Login()
        {
            string email = _periodicSetting.Email;
            string password = _periodicSetting.Pass;

            try
            {
                var response = _account.PeriodicAuthentication(new DTOs.Account.AuthenticationRequest
                {
                    Email = email,
                    Password = password
                });

                if (response.Succeeded)
                {
                    //log to file
                    var message = string.Format("Application Login works as at {0}", DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                    _logger.WriteLogToFile(message);
                }
                else
                {
                    //log error
                    var message = string.Format("Application Login failed as at {0} with no error message",
                        DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));

                    _logger.WriteLogToFile(message);

                    //send mail
                    var emailTemplate = "EmailTemplate/DevAlertMail.cshtml";
                    _emailService.SendFeedBackFluentEmailTemplate(new EmailRequest()
                    {
                        //To = _mailSettings.DeveloperEmail,
                        Subject = $"LPPC Failure Alert!",
                        Body = message,
                        //DeveloperEmail = _mailSettings.CopyDeveloperEmail
                    }, emailTemplate);

                }
            }
            catch (Exception ex)
            {
                //log error
                var message = string.Format("Application Login failed as at {0} with an error message {1} and inner error message {2}",
                    DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"), ex.Message, ex.InnerException?.Message);

                _logger.WriteLogToFile(message);
                _logger.LogExceptionToFile(ex);

                //send mail
                var emailTemplate = "EmailTemplate/DevAlertMail.cshtml";
                _emailService.SendFeedBackFluentEmailTemplate(new EmailRequest()
                {
                    //To = _mailSettings.DeveloperEmail,
                    Subject = $"LPPC Failure Alert!",
                    Body = message,
                    //DeveloperEmail = _mailSettings.CopyDeveloperEmail
                }, emailTemplate);
            }
        }


    }

    public interface IPeriodicLoginService
    {
        void Login();
    }
}
