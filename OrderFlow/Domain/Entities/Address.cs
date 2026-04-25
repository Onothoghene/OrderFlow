using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Address : AuditableBaseEntity
    {
        public Address()
        {
            Orders = new HashSet<Orders>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalPhoneNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }

        public UserProfile CreatedByNavigation { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }

}
