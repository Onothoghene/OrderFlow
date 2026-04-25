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
    public class ValidateDateWithinYrsCommand : IRequest<Response<bool>>
    {
        public string Date { get; set; }
        public int YearRange { get; set; }

        public class ValidateDateWithinYrsCommandHandler : IRequestHandler<ValidateDateWithinYrsCommand, Response<bool>>
        {
            private readonly IDateTimeService _dateTime;

            public ValidateDateWithinYrsCommandHandler(IDateTimeService dateTime)
            {
                _dateTime = dateTime;
            }
            public async Task<Response<bool>> Handle(ValidateDateWithinYrsCommand request, CancellationToken cancellationToken)
            {
                var startYear = DateTime.Now.AddYears(-request.YearRange).Year;
                var presentYear = DateTime.Now.Year;

                var docYear = _dateTime.EpocToDateTime(request.Date).Year;

                if (docYear < startYear || docYear > presentYear)
                {
                    throw new ApiException($"Please choose a date between {startYear} and {presentYear}");
                }

                return new Response<bool>(true, "Successful");
            }
        }
    }
}
