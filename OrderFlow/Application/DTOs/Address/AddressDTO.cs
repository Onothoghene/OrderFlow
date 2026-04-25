namespace Application.DTOs.Address
{
    public class AddressVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalPhoneNumber { get; set; }
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }

    }
    
    public class AddressIM
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
    }

    public class AddressEM : AddressIM
    {
        public int Id { get; set; }
    }

}
