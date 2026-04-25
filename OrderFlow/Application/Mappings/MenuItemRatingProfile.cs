using AutoMapper;
using System;

namespace Application.Mappings
{
    public class MenunItemRatingProfile : Profile
    {
        public MenunItemRatingProfile()
        {


        }

        public string DateTimeToEpoc(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds).ToString();
        }

    }
}
