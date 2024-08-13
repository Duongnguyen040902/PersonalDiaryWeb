namespace DuongNDH2_PersonalDiaryAPI.Models.Dto
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto User { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfPostLikes { get; set; }
        public string StatusPost { get; set; }
        public List<string> Photos { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }
}
