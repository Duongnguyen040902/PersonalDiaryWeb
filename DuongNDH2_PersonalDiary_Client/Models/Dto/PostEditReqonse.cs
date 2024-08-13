namespace DuongNDH2_PersonalDiary_Client.Models.Dto
{
    public class PostEditResponse
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto User { get; set; }
        public string StatusPost { get; set; }
        public List<string> Photos { get; set; }
    }
}
