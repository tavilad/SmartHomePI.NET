using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Data
{
    public class UserDetailsRepository : BaseRepository<UserDetails>, IUserDetailsRepository
    {
        private readonly DataContext context;
        public UserDetailsRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}