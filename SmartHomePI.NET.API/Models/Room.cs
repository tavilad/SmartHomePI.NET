namespace SmartHomePI.NET.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public User user { get; set; }
    }
}