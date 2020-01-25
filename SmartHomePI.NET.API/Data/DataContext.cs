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
    }
}