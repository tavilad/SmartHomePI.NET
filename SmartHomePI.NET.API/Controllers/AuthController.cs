using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartHomePI.NET.API.Data;
using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.DTOs;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMailService mailService;

        public AuthController(IAuthRepository repository, IConfiguration configuration, IMailService mailService)
        {
            this._configuration = configuration;
            this._repository = repository;
            this.mailService = mailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO user)
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

            await this.mailService.SendMailAsync(new MailRequest(){
                ToEmail = "pintiliciucoctavian@gmail.com",
                Subject = "Thank you for registering!",
                Body = $"Welcome {userToCreate.Username}! Thanks for creating an account on SmartHomePI.NET! Now you can start working on automating your home."
            });

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            User userFromRepo = await this._repository.Login(user.Username.ToLower(), user.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}