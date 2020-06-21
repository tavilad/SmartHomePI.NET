using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Data
{
    public class TemperatureAndHumidityRepository : BaseRepository<TemperatureAndHumidity>, ITemperatureAndHumidityRepository
    {
        private readonly DataContext context;
        public TemperatureAndHumidityRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}