using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuongNDH2_PersonalDiary_Client.Models
{
    public class PostLike
    {
        [Key]
        public int PostLikeId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Foreign key
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
