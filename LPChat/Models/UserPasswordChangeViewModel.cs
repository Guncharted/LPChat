using System;
using System.ComponentModel.DataAnnotations;

namespace LPChat.Services.ViewModels
{
	public class UserPasswordChangeViewModel
    {
        [Required]
        public Guid ID { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 20 знаков")]
        public string NewPassword { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
