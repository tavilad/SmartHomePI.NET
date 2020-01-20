using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHomePI.NET.API.Data;
using SmartHomePI.NET.API.DTOs;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        public AuthController(IAuthRepository repository)
        {
            this._repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserDTO user)
        {
            user.Username = user.Username.ToLower();

            if (await this._repository.UserExists(user.Username))
            {
                return BadRequest("Username already exists");
            }

            User userToCreate = new User
            {
                Username = user.Username
            };

            User createdUser = await this._repository.Register(userToCreate, user.Password);

            return StatusCode(201);
        }
    }
}