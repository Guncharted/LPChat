﻿using System.ComponentModel.DataAnnotations;

namespace LPChat.Services.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 20 знаков")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
