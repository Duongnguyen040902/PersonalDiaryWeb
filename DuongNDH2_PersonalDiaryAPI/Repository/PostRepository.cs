using Microsoft.EntityFrameworkCore;
using DuongNDH2_PersonalDiaryAPI.Models;
using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.Extensions.Hosting;

namespace DuongNDH2_PersonalDiaryAPI.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _environment;
        public PostRepository(MyDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<PostDto>> GetAllPublicPostsAsync()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.PostLikes)
                .Where(p => p.isPublish == true)
                .ToListAsync(); 

            var postDtos = posts.Select(post => new PostDto
            {
                PostId = post.PostId,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                User = new UserDto
                {
                    UserId = post.User.UserId,
                    UserName = post.User.UserName,
                    Email = post.User.Email,
                    ProfilePictureUrl = post.User.ProfilePictureUrl
                },
                NumberOfComments = post.Comments.Count,
                NumberOfPostLikes = post.PostLikes.Count,
                StatusPost = "Public",
                Photos = post.Photos.Select(photo => photo.Url).ToList()
            }).ToList();

            return postDtos;
        }
        public async Task<Post> CreatePostAsync(CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Content = postRequest.Content,
                CreatedAt = postRequest.CreatedAt,
                isPublish = postRequest.IsPublic,
                UserId = postRequest.UserId
            };

            // Process PhotoUrls if provided
            if (postRequest.PhotoUrls != null && postRequest.PhotoUrls.Count > 0)
            {
                foreach (var photo in postRequest.PhotoUrls)
                {
                   post.Photos.Add(new Photo { Url = "images/" + Path.GetFileName(photo.FileName) });
                   
                }
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<PostEditResponse> GetPostById(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.PostId == postId);

            var postEditResponse = new PostEditResponse
            {
                PostId = post.PostId,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                User = new UserDto
                {
                    UserId = post.User.UserId,
                    UserName = post.User.UserName,
                    Email = post.User.Email,
                    ProfilePictureUrl = post.User.ProfilePictureUrl
                },
                StatusPost = post.isPublish ? "Public" : "Private",
                Photos = post.Photos.Select(p => p.Url).ToList()
            };

            return postEditResponse;
        }

        public async Task<Post> UpdatePost(int postId, UpdatePostRequest updatePostRequest)
        {
            var post = await _context.Posts.Include(p => p.Photos).FirstOrDefaultAsync(p => p.PostId == postId);
            if (post == null)
            {
                return null; // or throw an exception, depending on your error handling strategy
            }

            // Update basic post informatio
            post.Content = updatePostRequest.Content;
            post.isPublish = updatePostRequest.IsPublic;

            // Handle photos
            if (updatePostRequest.PhotoUrls != null && updatePostRequest.PhotoUrls.Count > 0)
            {

                foreach (var photo in updatePostRequest.PhotoUrls)
                {
                    if (photo.Length > 0)
                    {
                        var filePath = Path.Combine("wwwroot/images", Path.GetFileName(photo.FileName));

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        post.Photos.Add(new Photo { Url = "images/" + Path.GetFileName(photo.FileName) });
                    }
                }
            }

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return post;
        }


        public async Task<bool> DeletePost(int postId, int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId && p.UserId == userId);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PostDto>> GetPostByUserIdAsync(int userId)
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Photos)
                .Include(p => p.PostLikes)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var postDtos = posts.Select(post => new PostDto
            {
                PostId = post.PostId,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                User = new UserDto
                {
                    UserId = post.User.UserId,
                    UserName = post.User.UserName,
                    Email = post.User.Email,
                    ProfilePictureUrl = post.User.ProfilePictureUrl,
                    DateOfBirth = post.User.DateOfBirth,
                    Gender = post.User.Gender
                },
                NumberOfComments = post.Comments.Count,
                NumberOfPostLikes = post.PostLikes.Count,
                StatusPost = post.isPublish ? "Public" : "Private",
                Photos = post.Photos.Select(photo => photo.Url).ToList()
            }).ToList();

            return postDtos;
        }
    }


}
