using DuongNDH2_PersonalDiaryAPI.Models;

namespace DuongNDH2_PersonalDiaryAPI.Repository.Imp
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
        Task<Comment> AddComment(int postId, int userId, string content);
        Task<bool> DeleteComment(int commentId, int userId);

    }
}
