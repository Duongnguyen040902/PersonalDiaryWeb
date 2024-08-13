using DuongNDH2_PersonalDiary_Client.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DuongNDH2_PersonalDiary_Client.Models.Dto
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and no more than 100 characters")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }

		public string? ProfilePictureUrl { get; set; }

	}
}
