using SmartHomePI.NET.API.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartHomePI.NET.API.DTOs
{
    public class RoomDTO
    {
        [Required]
        public string RoomName { get; set; }

        [Required]
        public int userId { get; set; }
    }
}