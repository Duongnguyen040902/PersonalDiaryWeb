using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DuongNDH2_PersonalDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _userRepository.Register(request);
            if (!result)
            {
                return BadRequest("Email already exists");
            }

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _userRepository.Login(request);
            if (response == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
            return Ok(userDto);
        }
    }
}
