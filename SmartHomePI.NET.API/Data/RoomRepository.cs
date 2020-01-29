using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Data
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly DataContext context;
        public RoomRepository(DataContext context) : base(context)
        {
            this.context = context;

        }
    }
}