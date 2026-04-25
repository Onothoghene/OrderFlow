using Domain.Common;
using System;

namespace Domain.Entities
{
    public class ContactUs : BaseEntity
    {
        public string Name { get; set; }
        public string EmailAddress { get;set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
