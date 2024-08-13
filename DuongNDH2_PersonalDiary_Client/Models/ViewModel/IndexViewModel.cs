using DuongNDH2_PersonalDiary_Client.Models.Dto;

namespace DuongNDH2_PersonalDiary_Client.Models.ViewModel
{
    public class IndexViewModel
    {
        public UserDto User { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}
