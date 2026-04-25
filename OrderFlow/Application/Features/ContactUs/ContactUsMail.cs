using Application.DTOs.Email;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Features.ContactUs
{
    public class ContactUsMail : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public class ContactUsMailHandler : IRequestHandler<ContactUsMail, Response<string>>
        {
            private readonly IContactUsRepositoryAsync _contactUs;
            private readonly IEmailService _email;
            private readonly MailSettings _mailSettings;

            public ContactUsMailHandler(IEmailService email, IOptionsSnapshot<MailSettings> mailSettings,
                IContactUsRepositoryAsync contactUs)
            {
                _contactUs = contactUs;
                _email = email;
                _mailSettings = mailSettings.Value;
            }

            public async Task<Response<string>> Handle(ContactUsMail command, CancellationToken cancellationToken)
            {
                using(TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var emailTemplate = "EmailTemplate/ContactUsMail.cshtml";

                    await _email.SendFeedBackFluentEmailTemplate(new EmailRequest()
                    {
                        //To = _mailSettings.ContactUsEmail,
                        Subject = $"{command.Name}'s Feedback",
                        FirstName = command.Name,
                        Body = command.Message,
                        FeedBackEmail = command.Email,
                        FeedBackPhoneNumber = command.PhoneNumber,
                        //CopyContactUsEmailTo = _mailSettings.CopyContactUsEmailTo
                    }, emailTemplate);

                    await _contactUs.AddAsync(new Domain.Entities.ContactUs
                    {
                        Name = command.Name,
                        PhoneNumber = command.PhoneNumber,
                        EmailAddress = command.Email,
                        Message = command.Message,
                        DateCreated = DateTime.Now,
                    });

                    ts.Complete();
                }

                return new Response<string>("message sent successfully");
            }
        }
    }
}
