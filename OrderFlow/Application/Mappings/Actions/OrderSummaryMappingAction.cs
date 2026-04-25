using Application.DTOs.Orders;
using Application.DTOs.Payment;
using Application.Enums;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappings.Actions
{
    public class OrderSummaryMappingAction : IMappingAction<Orders, OrderSummaryVM>
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;

        public OrderSummaryMappingAction(IMapper mapper, IDateTimeService dateTimeService)
        {
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public void Process(Orders source, OrderSummaryVM destination, ResolutionContext context)
        {
            destination.OrderDate = _dateTimeService.DateTimeToEpoc(source.Created);
            //destination.PaymentStatus = source.Payments != null && source.Payments.Count() > 0 ? source.Payments.Where(r => r.PaymentStatus == (int)PaymentStausEnum.Recieved).FirstOrDefault().PaymentStatus : 0;
            destination.PaymentStatus = source.Payments != null && source.Payments.Count() > 0 ? source.Payments.FirstOrDefault().PaymentStatus : 0;
            //destination.PaymentOption = source.Payments != null && source.Payments.Count() > 0  ? source.Payments.Where(r => r.PaymentStatus == (int)PaymentStausEnum.Recieved).FirstOrDefault().PaymentOption : 0;
            destination.PaymentOption = source.Payments != null && source.Payments.Count() > 0  ? source.Payments.FirstOrDefault().PaymentOption : 0;
            if(source.Payments != null && source.Payments.Count() > 0)
            {
                var res = source.Payments.Where(r => r.PaymentStatus == (int)PaymentStausEnum.Recieved).FirstOrDefault();
               destination.Payment = _mapper.Map<PaymentVM>(res);
            }
        }
    }

}
