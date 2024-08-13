using System.ComponentModel.DataAnnotations;

namespace DuongNDH2_PersonalDiary_Client.Models.Dto
{
    public class UpdatePostRequest
    {
        [Required]
        public int postId { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public List<IFormFile>? PhotoUrls { get; set; }
    }
}
