using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuongNDH2_PersonalDiaryAPI.Models.Dto
{
    public class CreatePostRequest
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public int UserId { get; set; }

        public List<IFormFile>? PhotoUrls { get; set; }

    }
}
