using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuongNDH2_PersonalDiaryAPI.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool isPublish { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

        // New navigation property for PostLikes
        public virtual ICollection<PostLike> PostLikes { get; set; }

        public Post()
        {
            Comments = new HashSet<Comment>();
            Photos = new HashSet<Photo>();
            PostLikes = new HashSet<PostLike>();
        }
    }
}
