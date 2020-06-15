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
        private Temperature lastReading;
        
        [HttpGet]
        public IActionResult GetTemperature()
        {
            using (Dht11 dht = new Dht11(PIN))
            {
                Temperature temperature = dht.Temperature;
                double humidity = dht.Humidity;

                if(dht.IsLastReadSuccessful){
                    return Ok(new{
                        temperature = temperature.Celsius.ToString(),
                        humidity = humidity.ToString()
                    });
                }
                else{
                    return NotFound("Could not read data from sensor");
                }

            }
        }
    }
}