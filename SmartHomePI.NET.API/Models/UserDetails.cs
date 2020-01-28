namespace SmartHomePI.NET.API.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public User user { get; set; }
    }
}