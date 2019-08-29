using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPChat.Core.DTO
{
    public class UserForRegister
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 20 знаков")]
        public string Password { get; set; }
    }
}
