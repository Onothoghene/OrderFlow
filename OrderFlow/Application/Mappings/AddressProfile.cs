using Application.DTOs.Address;
using Application.Features.Address.Command;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddOrUpdateAddressCommand, Address>();

            CreateMap<Address, AddressVM>();

        }


    }
}
