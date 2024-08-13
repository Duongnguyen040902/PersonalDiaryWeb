using Microsoft.AspNetCore.Mvc;
using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Models;
using Microsoft.EntityFrameworkCore;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DuongNDH2_PersonalDiaryAPI.Repository;

namespace DuongNDH2_PersonalDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly MyDBContext _context;

        public PostsController(IPostRepository postRepository, MyDBContext context)
        {
            _postRepository = postRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetAllPublicPosts()
        {
            var posts = await _postRepository.GetAllPublicPostsAsync();
            if(posts == null)
            {
                return NotFound("Posts not found");
            }
 
            return Ok(posts);
     
        }


        [HttpGet("user/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<PostDto>>> GetPostsByUserId(int userId)
        {
            var posts = await _postRepository.GetPostByUserIdAsync(userId);
            if (posts == null)
            {
                return NotFound("Posts not found");
            }
                
            return Ok(posts);
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Post>> CreatePost([FromForm] CreatePostRequest postRequest)
        {
            if (postRequest == null)
            {
                return BadRequest("Post request is null");
            }
            try
            {
                var post = await _postRepository.CreatePostAsync(postRequest);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{postId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PostEditResponse>> GetPostById(int postId)
        {
            var post = await _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            return Ok(post);
        }

        [HttpPut("{postId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdatePost(int postId, [FromForm] UpdatePostRequest updatePostRequest)
        {
            var updatedPost = await _postRepository.UpdatePost(postId, updatePostRequest);

            if (updatedPost == null)
            {
                return NotFound("Post not found or you do not have permission to edit this post.");
            }

            return Ok(updatedPost);
        }

        [HttpDelete("{postId}/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeletePost(int postId, int userId)
        {
            var result = await _postRepository.DeletePost(postId, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("statusPublic/{postId}")]
        public async Task<ActionResult> UpdateStatusPublic(int postId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == postId);

            if(post != null && post.isPublish == false)
            {
                post.isPublish = true; 
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return Ok("Done");
            }
            return BadRequest("NotDone");
            
        }
        [HttpPut("statusPrivate/{postId}")]
        public async Task<ActionResult> UpdateStatusPrivate(int postId)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == postId);

            if (post != null && post.isPublish == true)
            {
                post.isPublish = false;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
