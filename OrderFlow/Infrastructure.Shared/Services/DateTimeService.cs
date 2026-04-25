using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;

        public DateTime EpocToDateTime(string date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            //return epoch.AddSeconds(Convert.ToInt64(date));
            return epoch.AddMilliseconds(long.Parse(date));
        }

        public string DateTimeToEpoc(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            return Convert.ToInt64((date - epoch).TotalMicroseconds).ToString();
        }

        

    }
}
