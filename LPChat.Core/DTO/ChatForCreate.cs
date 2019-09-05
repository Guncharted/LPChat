using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LPChat.Core.DTO
{
    public class ChatForCreate
    {
        [Required]
        public IEnumerable<Guid> PersonIds { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
