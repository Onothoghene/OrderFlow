using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            var mailOption = _config.GetSection(nameof(MailSettings));

            services.Configure<MailSettings>(mailOption);
            services.AddSingleton(_config.GetSection(nameof(PayStackOptions)).Get<PayStackOptions>());

            services.Configure<ZipFileSettings>(_config.GetSection(nameof(ZipFileSettings)));
            services.Configure<RoundRobinSecUserSettings>(_config.GetSection(nameof(RoundRobinSecUserSettings)));
            services.Configure<PeriodicLoginSettings>(_config.GetSection(nameof(PeriodicLoginSettings)));
            services.Configure<ResourceLinkSettings>(_config.GetSection(nameof(ResourceLinkSettings)));
            services.Configure<OutlookCredentialsSettings>(_config.GetSection(nameof(OutlookCredentialsSettings)));

            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileZipService, FileZipService>();

            //var smtpClient = new SmtpClient
            //{
            //    Port = 587,
            //    EnableSsl = true,
            //    Host = "smtp.postmarkapp.com",

            //    //UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential("b8afc2e4-82c9-47ee-8ae2-493274715c53", "b8afc2e4-82c9-47ee-8ae2-493274715c53"),
            //};

            //services.AddFluentEmail(mailOption[nameof(MailSettings.EmailFrom)])
            //   //.AddSendGridSender(mailOption[nameof(MailSettings.ApiKey)])
            //   //.AddPostmarkSender(mailOption[nameof(MailSettings.ApiKey)])
            //   .AddSmtpSender(smtpClient)
            //   //.AddRazorRenderer();
            //   .AddRazorRenderer(Directory.GetCurrentDirectory());

            var client = new System.Net.Mail.SmtpClient();
            client.Credentials = new NetworkCredential(mailOption[nameof(MailSettings.SmtpUser)], mailOption[nameof(MailSettings.SmtpPass)]);
            client.Host = mailOption[nameof(MailSettings.SmtpHost)];
            client.Port = Convert.ToInt32(mailOption[nameof(MailSettings.SmtpPort)]);
            client.EnableSsl = Convert.ToBoolean(mailOption[nameof(MailSettings.EnableSsl)]);
            client.UseDefaultCredentials = false;
            //client.SendCompleted += (s, e) => client.Dispose();

            //services.AddFluentEmail(mailOption[nameof(MailSettings.EmailFrom)])
            //.AddSmtpSender(client) //NOTE: uncomment if you want to use smtp - recommended for other email delivery services e.g PostMark
            //                       //.AddSendGridSender(mailOption[nameof(MailSettings.ApiKey)]) //NOTE: uncomment if you want to use sendgrid
            //.AddRazorRenderer(Directory.GetCurrentDirectory());

            //var client = new SmtpClient(mailOption[nameof(MailSettings.SmtpHost)],
            //                Convert.ToInt32(mailOption[nameof(MailSettings.SmtpPort)]));

            //client.Credentials = new NetworkCredential(
            //    mailOption[nameof(MailSettings.SmtpUser)],
            //    mailOption[nameof(MailSettings.SmtpPass)]
            //);

            //client.EnableSsl = false; // Disable SSL (Zoho requires STARTTLS)
            //client.UseDefaultCredentials = false;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //// Explicitly enable STARTTLS after connecting
            //client.SendCompleted += (s, e) => client.Dispose();

            // Manually enable SSL after the connection is established
            client.EnableSsl = true;




            services.AddFluentEmail(mailOption[nameof(MailSettings.EmailFrom)])
                .AddSmtpSender(client)
                .AddRazorRenderer(Directory.GetCurrentDirectory());


            services.AddSingleton<IFileUploadService, FileUploadService>();

        }
    }
}
