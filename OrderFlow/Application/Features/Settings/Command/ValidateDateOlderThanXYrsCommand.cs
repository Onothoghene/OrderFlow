using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Settings.Command
{
    public class ValidateDateOlderThanXYrsCommand : IRequest<Response<bool>>
    {
        public string Date { get; set; }
        public int YearRange { get; set; }

        public class ValidateDateOlderThanXYrsCommandHandler : IRequestHandler<ValidateDateOlderThanXYrsCommand, Response<bool>>
        {
            private readonly IDateTimeService _dateTime;

            public ValidateDateOlderThanXYrsCommandHandler(IDateTimeService dateTime)
            {
                _dateTime = dateTime;
            }
            public async Task<Response<bool>> Handle(ValidateDateOlderThanXYrsCommand request, CancellationToken cancellationToken)
            {
                var startYear = DateTime.Now.AddYears(-request.YearRange).Year;

                var docYear = _dateTime.EpocToDateTime(request.Date).Year;

                if (docYear > startYear)
                {
                    throw new ApiException($"Please choose a date older than or equal to {startYear}");
                }

                return new Response<bool>(true, "Successful");
            }
        }
    }
}
