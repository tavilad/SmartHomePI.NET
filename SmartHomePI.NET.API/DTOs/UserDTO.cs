using System.ComponentModel.DataAnnotations;

namespace SmartHomePI.NET.API.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Password must have between 4 and 15 characters")]
        public string Password { get; set; }
        
    }
}