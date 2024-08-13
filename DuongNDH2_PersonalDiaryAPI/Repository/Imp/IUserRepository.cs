using DuongNDH2_PersonalDiaryAPI.Models;
using DuongNDH2_PersonalDiaryAPI.Models.Dto;

namespace DuongNDH2_PersonalDiaryAPI.Repository.Imp
{
    public interface IUserRepository
    {
        Task<bool> Register(RegisterRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<bool> IsEmailExist(string email);
        Task<User> GetUserById(int userId);
    }
}
