using Iot.Device.Hcsr04;
using Microsoft.AspNetCore.Mvc;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistanceController : ControllerBase
    {
        private const int ECHO = 18;
        private const int TRIG = 16;


        [HttpGet]
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