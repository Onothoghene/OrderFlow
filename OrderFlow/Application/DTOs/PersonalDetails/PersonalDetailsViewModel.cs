namespace Application.DTOs.PersonalDetails
{
    public class PersonalDetailsVM
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
    
}
