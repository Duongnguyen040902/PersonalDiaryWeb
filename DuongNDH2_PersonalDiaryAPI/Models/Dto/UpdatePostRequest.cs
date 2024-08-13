using System.ComponentModel.DataAnnotations;

namespace DuongNDH2_PersonalDiaryAPI.Models.Dto
{
    public class UpdatePostRequest
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public List<IFormFile>? PhotoUrls { get; set; }
    }
}
