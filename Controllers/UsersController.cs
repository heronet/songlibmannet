using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;

namespace Controllers
{
    public class UsersController : BaseAPIController
    {
        private readonly UserManager<EndUser> _userManager;
        public UsersController(UserManager<EndUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("users")]
        public async Task<ActionResult<EndUser>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest("User Not Found");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            EndUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return Ok(new UserDTO
                {
                    Username = user.UserName,
                    Token = "Token",
                    ImageUrl = null
                });
            }
            return BadRequest("User Not Found");
        }

    }
}