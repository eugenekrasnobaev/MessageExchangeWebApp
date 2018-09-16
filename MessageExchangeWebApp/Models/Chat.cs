using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageExchangeWebApp.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        
        public int? UserIdSrc { get; set; }
        [ForeignKey("UserIdSrc")]
        public User UserSrc { get; set; }
        
        public int? UserIdDst { get; set; }
        [ForeignKey("UserIdDst")]
        public User UserDst { get; set; }

        public ICollection<Message> Messages { get; set; }
        public Chat()
        {
            Messages = new List<Message>();
        }

    }
}