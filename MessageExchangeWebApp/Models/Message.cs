using System;
using System.ComponentModel.DataAnnotations;

namespace MessageExchangeWebApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }

        public int? ChatId { get; set; }
        public Chat Chat { get; set; }

    }
}