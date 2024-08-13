using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DuongNDH2_PersonalDiaryAPI.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }


}