namespace SmartHomePI.NET.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string IpAddress { get; set; }
        public virtual User user { get; set; }
        public int UserId { get; set; }
    }
}