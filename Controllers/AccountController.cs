using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Repository;

namespace Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly IEndUserRepository _repository;
        public AccountController(IEndUserRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EndUser>> GetUser(Guid id)
        {
            EndUser user = await _repository.GetUserByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("User Not Found");
        }
        [HttpPost("signup")]
        public async Task<ActionResult<EndUser>> CreateEndUser(SignupDTO signupDTO)
        {
            if (await CheckIfUserExists(signupDTO)) return BadRequest("Username or Email already exists");

            using var hmac = new HMACSHA512();

            EndUser user = new EndUser
            {
                Name = signupDTO.Name,
                Email = signupDTO.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signupDTO.Password)),
                PasswordSalt = hmac.Key
            };
            if (await _repository.CreateEndUserAsync(user))
                return Ok(user);
            return BadRequest("User Creation Failed");
        }
        [HttpPost("signin")]
        public async Task<ActionResult<EndUser>> LoginEndUser(SignInDTO signInDTO)
        {
            EndUser user = await _repository.GetUserByEmailAsync(signInDTO.Email);

            if (user != null)
            {
                using var hmac = new HMACSHA512(user.PasswordSalt);
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signInDTO.Password));
                for (uint i = 0; i != computedHash.Length; ++i)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                        return BadRequest("Invalid Password");
                }
                return Ok(user);
            }
            return BadRequest("Invalid Password");

        }

        private async Task<bool> CheckIfUserExists(SignupDTO signupDTO)
        {
            EndUser user_by_name = await _repository.GetUserByUsernameAsync(signupDTO.Name);
            if (user_by_name != null)
                return true;
            EndUser user_by_email = await _repository.GetUserByEmailAsync(signupDTO.Email);
            if (user_by_email != null)
                return true;
            return false;
        }
    }
}