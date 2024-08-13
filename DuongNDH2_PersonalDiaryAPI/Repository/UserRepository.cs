using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Models;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DuongNDH2_PersonalDiaryAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDBContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(MyDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            if (await IsEmailExist(request.Email))
            {
                return false; 
            }
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Role = Role.User,
                ProfilePictureUrl = request.ProfilePictureUrl
			};

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim("UserId", user.UserId.ToString()),
                     new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                     new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                     new Claim("DateOfBirth", user.DateOfBirth.ToString()),
                     new Claim(ClaimTypes.Gender, user.Gender.ToString()),
                     new Claim(ClaimTypes.Role, user.Role.ToString()),
                     new Claim(ClaimTypes.Uri, user.ProfilePictureUrl ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponse { Token = tokenHandler.WriteToken(token) };
        }
        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users
            .Include(u => u.Posts)
            .SingleOrDefaultAsync(u => u.UserId == userId);
        }

    }

}
