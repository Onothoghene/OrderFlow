using Application.DTOs.Payment;
using Application.Enums;
using Application.Features.Comment.Command;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<AddOrUpdatePaymentCommand, Payment>();
            CreateMap<Payment, PaymentVM>()
                 .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => ((PaymentStausEnum)src.PaymentStatus).ToString()))
                 .ForMember(dest => dest.PaymentStatusId, opt => opt.MapFrom(src => src.PaymentStatus))
                 .ForMember(dest => dest.PaymentOptionId, opt => opt.MapFrom(src => src.PaymentOption))
                 .ForMember(dest => dest.PaymentOption, opt => opt.MapFrom(src => ((PaymentOptionEnum)src.PaymentOption).ToString()));

        }

    }
}
