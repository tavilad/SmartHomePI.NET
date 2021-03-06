using System;
using System.Collections.Generic;
using System.Linq;
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
        private const int PIN = 12;
        private readonly ITemperatureAndHumidityRepository repository;
        private readonly DataContext context;
        private IMailService mailService;

        public TemperatureController(ITemperatureAndHumidityRepository repository, DataContext context, IMailService mailService)
        {
            this.context = context;
            this.repository = repository;
            this.mailService = mailService;
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetTemperature(int roomId)
        {
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
                        DayOfReading = DateTime.Today,
                        RoomId = roomId
                    });

                    return Ok(new
                    {
                        temperature = temperature.Celsius.ToString(),
                        humidity = humidity.ToString()
                    });
                }
                else
                {
                    IEnumerable<TemperatureAndHumidity> temps = await this.repository.Get(temp => temp.RoomId == roomId);
                    if (temps.FirstOrDefault() != null)
                    {
                        return Ok(new
                        {
                            temperature = temps.FirstOrDefault().Temperature,
                            humidity = temps.FirstOrDefault().Humidity
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

        [HttpGet("Report/{userName}/{room}")]
        public async Task<IActionResult> SendReport(string userName, int roomId)
        {
            IEnumerable<TemperatureAndHumidity> data = await this.repository.Get(temp => temp.DayOfReading == DateTime.Today && temp.RoomId == roomId, null, "");
            double averageTemperature = data.Average(temp => temp.Temperature);
            double averageHumidity = data.Average(hum => hum.Humidity);
            Console.WriteLine("report");
            await this.mailService.SendMailAsync(new MailRequest()
            {
                ToEmail = "pintiliciucoctavian@gmail.com",
                Subject = $"Room report for {DateTime.Today.Date}",
                Body = $"Hello {userName}! Here is your report for today for the room {roomId}: Temperature average - {averageTemperature}, Humidity average - {averageHumidity}."
            });

            return Ok();
        }
    }
}