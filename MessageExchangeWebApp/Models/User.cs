using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageExchangeWebApp.Models
{
    public class User 
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        
        [InverseProperty("UserSrc")]
        public ICollection<Chat> ChatsSrc { get; set; }

        [InverseProperty("UserDst")]
        public ICollection<Chat> ChatsDst { get; set; }

        public User()
        {
            ChatsSrc = new List<Chat>();
            ChatsDst = new List<Chat>();
        }
        

    }
}