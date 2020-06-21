using Microsoft.EntityFrameworkCore;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<TemperatureAndHumidity> TemperatureAndHumidities{get;set;}
    }
}