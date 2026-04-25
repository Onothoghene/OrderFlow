using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class UserProfile : AuditableBaseEntity
    {
        public UserProfile()
        {
            Addresses = new HashSet<Address>();
            Orders = new HashSet<Orders>();
            CartItems = new HashSet<CartItems>();
        }


        public int? GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int VerificationCode { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public ICollection<CartItems> CartItems { get; set; }

    }
}
