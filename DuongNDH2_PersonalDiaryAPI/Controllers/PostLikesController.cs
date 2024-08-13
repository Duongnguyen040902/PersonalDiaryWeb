using DuongNDH2_PersonalDiaryAPI.Models;
using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DuongNDH2_PersonalDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikesController : ControllerBase
    {
        private readonly MyDBContext _context;

        public PostLikesController(MyDBContext context)
        {
            _context = context;
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<PostLike>>> GetPostLikeByPostId(int postId)
        {
            return await _context.PostLikes.Where(l => l.PostId == postId).Include(u => u.User).ToListAsync();

        }

        [HttpGet("{userId}/{postId}")]
        public async Task<ActionResult<bool>> CheckIfUserLikedPost(int userId, int postId)
        {
            var postLike = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            bool hasLiked = postLike != null;

            return Ok(hasLiked);
        }

        [HttpPost("{userId}/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PostLike>> LikePost(int userId, int postId)
        {
            var existingLike = await _context.PostLikes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            // Create a new PostLike entry
            var newLike = new PostLike
            {
                CreatedAt = DateTime.Now,
                UserId = userId,
                PostId = postId
            };

            _context.PostLikes.Add(newLike);
            await _context.SaveChangesAsync();

            return Ok();
        }
          
        [HttpDelete("{userId}/{postId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PostLike>> UnlikePost(int userId, int postId)
        {
            var postLike = await _context.PostLikes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
            if (postLike == null)
            {
                return NotFound("User has not liked this post.");
            }

            _context.PostLikes.Remove(postLike);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
