using System.ComponentModel.DataAnnotations;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.DTOs
{
    public class UserDetailsDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]
        public int userId { get; set; }
    }
}