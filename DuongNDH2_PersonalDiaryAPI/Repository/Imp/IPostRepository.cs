using DuongNDH2_PersonalDiaryAPI.Models.Dto;
using DuongNDH2_PersonalDiaryAPI.Models;

namespace DuongNDH2_PersonalDiaryAPI.Repository.Imp
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetAllPublicPostsAsync();
        Task<List<PostDto>> GetPostByUserIdAsync(int userId);
        Task<Post> CreatePostAsync(CreatePostRequest post);
        Task<PostEditResponse> GetPostById(int postId);
        Task<Post> UpdatePost(int postId, UpdatePostRequest updatePostRequest);
        Task<bool> DeletePost(int postId, int userId);
    }
}
