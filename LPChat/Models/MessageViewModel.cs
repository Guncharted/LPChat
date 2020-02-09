using System;
using System.ComponentModel.DataAnnotations;

namespace LPChat.Infrastructure.ViewModels
{
    public class MessageViewModel
    {
        public Guid? ID { get; set; }

        [Required]
        public Guid? ChatId { get; set; }

        public Guid? PersonId { get; set; }

        public string PersonName { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedUtcDate { get; set; }
    }
}
