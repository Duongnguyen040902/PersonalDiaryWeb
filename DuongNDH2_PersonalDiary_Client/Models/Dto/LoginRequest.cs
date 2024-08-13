﻿using System.ComponentModel.DataAnnotations;

namespace DuongNDH2_PersonalDiary_Client.Models.Dto
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and no more than 100 characters")]
        public string Password { get; set; }
    }
}
