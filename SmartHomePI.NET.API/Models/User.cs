using System.Collections.Generic;

namespace SmartHomePI.NET.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<UserDetails> UserDetails { get; set; }
    }
}