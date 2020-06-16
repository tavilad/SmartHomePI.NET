using System.Threading.Tasks;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Data.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task DeleteByName(string roomName);
    }
}