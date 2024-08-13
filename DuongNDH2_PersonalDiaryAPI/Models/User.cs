using DuongNDH2_PersonalDiaryAPI.Models;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuongNDH2_PersonalDiaryAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; } = Role.User; 
        public string? ProfilePictureUrl { get; set; }

        // Navigation properties
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }

        public User()
        {
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            PostLikes = new HashSet<PostLike>();
        }
    }
}
