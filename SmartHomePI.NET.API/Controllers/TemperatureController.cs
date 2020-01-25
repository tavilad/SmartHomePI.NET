using Iot.Device.DHTxx;
using Iot.Units;
using Microsoft.AspNetCore.Mvc;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private const int PIN = 4;

        [HttpGet]
        private IActionResult GetTemperature()
        {
            using (Dht11 dht = new Dht11(PIN))
            {
                Temperature temperature = dht.Temperature;
                double humidity = dht.Humidity;

                return Ok(new{
                    temperature = temperature.Celsius,
                    humidity = humidity
                });
            }
        }
    }
}