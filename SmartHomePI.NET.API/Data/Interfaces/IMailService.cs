using System;
using System.Threading.Tasks;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Data.Interfaces
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest mailRequest);
    }
}