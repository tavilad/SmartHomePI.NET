using System;
using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Iot.Units;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomePI.NET.API.Data;
using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.Models;

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

        private readonly ITemperatureAndHumidityRepository repository;
        private readonly DataContext context;

        public TemperatureController(ITemperatureAndHumidityRepository repository, DataContext context)
        {
            this.context = context;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTemperature(int roomId)
        {
            if (devEnv)
            {
                Random random = new Random();
                double temperature = (random.NextDouble() * (maxWeather - minWeather) + minWeather);
                double humidity = (random.NextDouble() * (maxHumidity - minHumidity) + minHumidity);
                await this.repository.Insert(new TemperatureAndHumidity()
                {
                    Temperature = temperature,
                    Humidity = humidity,
                    DayOfReading = DateTime.Today
                });

                return Ok(new
                {
                    temperature = temperature.ToString(),
                    humidity = humidity.ToString()
                });
            }

            using (Dht11 dht = new Dht11(PIN))
            {
                Temperature temperature = dht.Temperature;
                double humidity = dht.Humidity;

                if (temperature.Celsius.ToString() != "NaN" && humidity.ToString() != "NaN")
                {
                    await this.repository.Insert(new TemperatureAndHumidity()
                    {
                        Temperature = temperature.Celsius,
                        Humidity = humidity,
                        DayOfReading = DateTime.Today
                    });

                    return Ok(new
                    {
                        temperature = temperature.Celsius.ToString(),
                        humidity = humidity.ToString()
                    });
                }
                else
                {
                    return Ok(new
                    {
                        temperature = "",
                        humidity = ""
                    });
                }
            }
        }
    }
}