using Application.DTOs.PersonalDetails;
using Application.Enums;
using Application.Mappings.Actions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappings
{
    public class PersonalDetailsProfile : Profile
    {
        public PersonalDetailsProfile()
        {
            CreateMap<UserProfile, PersonalDetailsVM>()
                  .AfterMap<PersonalDetailsMappingAction>();

           }

        public string DateTimeToEpoc(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds).ToString();
        }

    }
}
