using Iot.Device.DHTxx;
using Iot.Units;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private const int PIN = 4;

        public TemperatureController()
        {

        }
        
        [HttpGet]
        public IActionResult GetTemperature()
        {
            using (Dht11 dht = new Dht11(PIN))
            {
                Temperature temperature = dht.Temperature;
                double humidity = dht.Humidity;

                return Ok(new{
                    temperature = temperature.Celsius.ToString(),
                    humidity = humidity.ToString()
                });
            }
        }
    }
}