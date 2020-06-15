using System.Threading.Tasks;
using Iot.Device.DHTxx;
using Iot.Units;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Peripherals;
using Unosquare.WiringPi;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private const int PIN = 4;
        private Temperature lastSuccessfulReadingTemperature;
        private double temperature;
        private double humidity;

        public TemperatureController()
        {
            Pi.Init<BootstrapWiringPi>();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTemperature()
        {
            using(var sensor = DhtSensor.Create(DhtType.Dht11, Pi.Gpio[BcmPin.Gpio04]))
            {
                sensor.Start();
                sensor.OnDataAvailable += (s, e) =>
                {
                    if (!e.IsValid)
                        return;
                    this.temperature = e.Temperature;
                    this.humidity = e.HumidityPercentage;
                };
                
                return Ok(new {
                    temperature = this.temperature.ToString(),
                    humidity = this.humidity.ToString()
                });
            }
        }
    }
}