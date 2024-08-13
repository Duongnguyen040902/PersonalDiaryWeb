using DuongNDH2_PersonalDiary_Client.Models.Dto;

namespace DuongNDH2_PersonalDiaryClient.Models.ViewModel
{
    public class EditPostViewModel
    {
        public PostEditResponse PostEditResponse { get; set; }
        public UpdatePostRequest UpdatePostRequest { get; set; }
        public string ReturnUrl { get; set; }
    }
}
