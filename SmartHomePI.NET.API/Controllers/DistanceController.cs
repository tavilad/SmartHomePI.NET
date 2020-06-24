using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
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
        private GpioController controller;

        public DistanceController()
        {
            this.controller = new GpioController();
            this.controller.OpenPin(TRIG, PinMode.Output);
            this.controller.OpenPin(ECHO, PinMode.Input);
            this.controller.Write(TRIG,PinValue.Low);
        }

        [HttpGet("get")]
        public IActionResult GetDistance()
        {
            return Ok(new{distance = Distance()});
        }

        public double Distance()
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(500);
            Stopwatch pulseLength = new Stopwatch();

            //Send pulse
            this.controller.Write(TRIG,PinValue.High);
            mre.WaitOne(TimeSpan.FromMilliseconds(0.01));
            this.controller.Write(TRIG,PinValue.Low);

            //Recieve pusle
            while (this.controller.Read(ECHO) == PinValue.Low)
            {
                pulseLength.Start();
            }

            while (this.controller.Read(ECHO) == PinValue.High)
            {
                pulseLength.Stop();
            }

            //Calculating distance
            TimeSpan timeBetween = pulseLength.Elapsed;
            Debug.WriteLine(timeBetween.ToString());
            double distance = timeBetween.TotalSeconds * 17000;

            return distance;
        }
    }
}