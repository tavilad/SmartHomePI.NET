using System.Threading.Tasks;
using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SmartHomePI.NET.API.Data
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly DataContext context;
        public RoomRepository(DataContext context) : base(context)
        {
            this.context = context;

        }

        public async Task DeleteByName(string roomName)
        {
            var dbSet = this.context.Set<Room>();

            Room entity = dbSet.FirstOrDefault(room => room.RoomName.Equals(roomName));

            if (this.context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);

            await this.context.SaveChangesAsync();
        }
    }
}