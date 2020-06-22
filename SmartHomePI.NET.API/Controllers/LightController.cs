using System.Device.Gpio;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightController : ControllerBase
    {
        private const int PIN = 17;
        private GpioController controller;

        public LightController()
        {
            this.controller = new GpioController();
        }

        [HttpGet("LightOn")]
        public IActionResult TurnOnLight()
        {
            this.controller.OpenPin(PIN, PinMode.Output);
            this.controller.Write(PIN,PinValue.High);
            return Ok();
        }

        [HttpGet("LightOff")]
        public IActionResult TurnOffLight()
        {
            this.controller.Write(PIN,PinValue.Low);
            this.controller.ClosePin(PIN);
            return Ok();
        }
    }
}