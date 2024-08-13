using DuongNDH2_PersonalDiaryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DuongNDH2_PersonalDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly MyDBContext _context;

        public PhotosController(MyDBContext context)
        {
            _context = context;
        }

        [HttpDelete("Photos/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var photo = await _context.Photos.FindAsync(photoId);
            if (photo == null)
            {
                return NotFound();
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("deleteByPostIdAndUrl")]
        public async Task<IActionResult> DeletePhotoByPostIdAndUrl(int postId, string url)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.PostId == postId && p.Url == url);
            if (photo == null)
            {
                return NotFound("Photo not found.");
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
