using System;
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
        private bool devEnv = true;
        private const double maxWeather = 24.0;
        private const double minWeather = 25.0;
        private const double maxHumidity = 65.0;
        private const double minHumidity = 70.0;
        
        [HttpGet]
        public IActionResult GetTemperature()
        {
            if(devEnv)
            {
                Random random = new Random();
                return Ok(new{
                    temperature = (random.NextDouble() * (maxWeather - minWeather) + minWeather).ToString(),
                    humidity = (random.NextDouble() * (maxHumidity - minHumidity) + minHumidity).ToString()
                });
            }

            using (Dht11 dht = new Dht11(PIN))
            {
                Temperature temperature = dht.Temperature;
                double humidity = dht.Humidity;
                
                if(temperature.Celsius.ToString() != "NaN" && humidity.ToString() != "NaN")
                {
                    return Ok(new{
                        temperature = temperature.Celsius.ToString(),
                        humidity = humidity.ToString()
                    });
                }
                else
                {
                    return Ok(new{
                        temperature = "",
                        humidity = ""
                    });
                }
            }
        }
    }
}