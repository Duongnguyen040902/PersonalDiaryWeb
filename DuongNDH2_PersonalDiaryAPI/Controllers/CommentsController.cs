using DuongNDH2_PersonalDiaryAPI.Models;
using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace DuongNDH2_PersonalDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPostId(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostId(postId);
            var commentsDto = comments.Select(comment => new CommentDto
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                User = new UserDto
                {
                    UserId = comment.User.UserId,
                    UserName = comment.User.UserName,
                    Email = comment.User.Email,
                    ProfilePictureUrl = comment.User.ProfilePictureUrl
                },
            }).ToList();

            return Ok(commentsDto);
        }

        [HttpPost("{postId}/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Comment>> AddComment(int postId, int userId, [FromBody] string content)
        {
            var createdComment = await _commentRepository.AddComment(postId, userId, content);
            return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = createdComment.PostId }, createdComment);
        }

        [HttpDelete("{commentId}/{userId}")]
        public async Task<IActionResult> DeleteComment(int commentId, int userId)
        {
            var result = await _commentRepository.DeleteComment(commentId, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
