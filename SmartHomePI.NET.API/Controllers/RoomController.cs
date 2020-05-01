using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomePI.NET.API.Data;
using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.DTOs;
using SmartHomePI.NET.API.Models;
using System.Linq;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository repository;
        private readonly DataContext context;

        public RoomController(IRoomRepository repository, DataContext context)
        {
            this.context = context;
            this.repository = repository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom(RoomDTO room)
        {
            await this.repository.Insert(new Room
            {
                RoomName = room.RoomName,
                user = await this.context.Users.FindAsync(room.userId)
            });

            return StatusCode(201);
        }

        [HttpGet("Get")]
        public async Task<IEnumerable<RoomDTO>> GetRooms()
        {
            IEnumerable<Room> rooms = await this.repository.Get(room => room != null, null, "user");
            List<RoomDTO> roomsToReturn = new List<RoomDTO>();

            foreach (Room room in rooms)
            {
                roomsToReturn.Add(new RoomDTO
                {
                    RoomName = room.RoomName,
                    userId = room.user.Id
                });
            }

            return roomsToReturn;
        }

        [HttpGet("Get/{roomId}")]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            IEnumerable<Room> roomList = await this.repository.Get(room => room.Id == roomId, null, "user");

            Room roomToReturn = roomList.FirstOrDefault();

            if (roomToReturn != null)
            {
                return Ok(new
                {
                    RoomName = roomToReturn.RoomName,
                    userId = roomToReturn.user.Id
                });
            }
            else
            {
                return BadRequest("The room requested does not exist.");
            }
        }

        [HttpGet("Get/forUser/{userId}")]
        public async Task<IActionResult> GetRoomByUserId(int userId)
        {
            IEnumerable<Room> roomList = await this.repository.Get(room => room.UserId == userId, null);

            if (roomList != null)
            {
                return Ok(new
                {
                    roomList
                });
            }
            else
            {
                return BadRequest("The rooms requested for userID "+ userId +" do not exist");
            }
        } 

        [HttpDelete("Delete/{roomId}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            await this.repository.Delete(roomId);
            return StatusCode(200);
        }

        // [HttpPut("Update/{roomId}")]
        // public async Task<IActionResult> UpdateRoom(int roomId)
        // {
            
        // }
    }
}