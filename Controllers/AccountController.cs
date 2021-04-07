using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Services;

namespace Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<EndUser> _userManager;
        private readonly SignInManager<EndUser> _signInManager;
        private readonly JwtTokenService _tokenService;
        public AccountController(UserManager<EndUser> userManager, SignInManager<EndUser> signInManager, JwtTokenService tokenService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpPost("signup")]
        public async Task<ActionResult<IdentityResult>> CreateEndUser(SignupDTO signupDTO)
        {
            EndUser user = new EndUser
            {
                UserName = signupDTO.Name,
                Email = signupDTO.Email
            };
            var result = await _userManager.CreateAsync(user, signupDTO.Password);
            if (result.Succeeded)
            {
                return Ok(returnRTO(user));
            }
            return BadRequest(result);

        }
        [HttpPost("signin")]
        public async Task<ActionResult<UserDTO>> LoginEndUser(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByEmailAsync(signInDTO.Email);
            if (user == null) return BadRequest("Invalid Email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, signInDTO.Password, false);

            if (result.Succeeded)
            {
                return Ok(returnRTO(user));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetAuthorizedUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return Ok(returnRTO(user));
        }
        private UserDTO returnRTO(EndUser user)
        {
            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                ImageUrl = null
            };
        }
    }
}