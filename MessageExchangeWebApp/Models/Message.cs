using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageExchangeWebApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
        public string Date { get; set; }

        public int? ChatId { get; set; }
        public Chat Chat { get; set; }

    }
}