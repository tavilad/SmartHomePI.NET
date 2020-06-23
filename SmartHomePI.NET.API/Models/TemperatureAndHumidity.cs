using System;

namespace SmartHomePI.NET.API.Models
{
    public class TemperatureAndHumidity
    {
        public int Id { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public DateTime DayOfReading { get; set; }

        public int RoomId { get; set; }
    }
}