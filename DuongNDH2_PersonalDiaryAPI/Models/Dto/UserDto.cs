namespace DuongNDH2_PersonalDiaryAPI.Models.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
    }
}
