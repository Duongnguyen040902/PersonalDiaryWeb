using DuongNDH2_PersonalDiaryAPI.Models;
using DuongNDH2_PersonalDiaryAPI.Repository.Imp;
using Microsoft.EntityFrameworkCore;

namespace DuongNDH2_PersonalDiaryAPI.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyDBContext _context;

        public CommentRepository(MyDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .ToListAsync();
        }
        public async Task<Comment> AddComment(int postId, int userId, string content)
        {
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
        public async Task<bool> DeleteComment(int commentId, int userId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.CommentId == commentId && c.UserId == userId);

            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
