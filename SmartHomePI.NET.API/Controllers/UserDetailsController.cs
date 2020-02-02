using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHomePI.NET.API.Data;
using SmartHomePI.NET.API.Data.Interfaces;
using SmartHomePI.NET.API.DTOs;
using SmartHomePI.NET.API.Models;

namespace SmartHomePI.NET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsRepository repository;
        private readonly DataContext context;

        public UserDetailsController(IUserDetailsRepository repository, DataContext context)
        {
            this.context = context;
            this.repository = repository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserDetails(UserDetailsDTO userDetails)
        {
            await this.repository.Insert(new UserDetails
            {
                Email = userDetails.Email,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                user = await this.context.Users.FindAsync(userDetails.userId)
            });

            return StatusCode(201);
        }

        [HttpGet("Get")]
        public async Task<IEnumerable<UserDetailsDTO>> GetUserDetails()
        {
            IEnumerable<UserDetails> userDetails = await this.repository.Get(room => room != null, null, "user");
            List<UserDetailsDTO> userDetailsToReturn = new List<UserDetailsDTO>();

            foreach (UserDetails userDetail in userDetails)
            {
                userDetailsToReturn.Add(new UserDetailsDTO
                {
                    Email = userDetail.Email,
                    FirstName = userDetail.FirstName,
                    LastName = userDetail.LastName,
                    userId = userDetail.user.Id
                });
            }

            return userDetailsToReturn;
        }

        [HttpGet("Get/{userDetailsId}")]
        public async Task<IActionResult> GetUserDetailsById(int userDetailsId)
        {
            IEnumerable<UserDetails> userDetailList = 
            await this.repository.Get(userDetail => userDetail.Id == userDetailsId, null, "user");

            UserDetails userDetailsToReturn = userDetailList.FirstOrDefault();

            if (userDetailsToReturn != null)
            {
                return Ok(new
                {
                    Email = userDetailsToReturn.Email,
                    FirstName = userDetailsToReturn.FirstName,
                    LastName = userDetailsToReturn.LastName,
                    userId = userDetailsToReturn.user.Id
                });
            }
            else
            {
                return BadRequest("The room requested does not exist.");
            }
        }

        // [HttpDelete]
        // public async Task<IActionResult> DeleteUserDetails(int userDetailsId)
        // {

        // }

        // [HttpPut]
        // public async Task<IActionResult> UpdateUserDetails(int userDetailsId)
        // {

        // }
    }
}