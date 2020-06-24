using System.Device.Gpio;
using Iot.Device.Hcsr04;
using Microsoft.AspNetCore.Mvc;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistanceController : ControllerBase
    {
        private const int ECHO = 24;
        private const int TRIG = 23;

        [HttpGet("get")]
        public IActionResult GetDistance()
        {
            using (var sonar = new Hcsr04(TRIG, ECHO))
            {
                return Ok(new
                {
                    distance = sonar.Distance.ToString(),
                });
            }
        }
    }
}