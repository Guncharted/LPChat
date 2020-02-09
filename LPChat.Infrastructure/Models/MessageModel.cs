﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LPChat.Infrastructure.Models
{
    public class MessageModel
    {
        public Guid? ID { get; set; }

        public Guid? ChatId { get; set; }

        public Guid? PersonId { get; set; }

        public string PersonName { get; set; }

        public string Text { get; set; }

        public DateTime CreatedUtcDate { get; set; }
    }
}
