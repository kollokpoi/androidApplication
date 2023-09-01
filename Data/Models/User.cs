using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomApi.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(10)]
        public string LastName { get; set; } = "";

        [StringLength(20)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(20)]
        [JsonIgnore]
        public string? Username { get; set; }

        [Required]
        [JsonIgnore]
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime LastOnline { get; set; }


        public virtual List<UserImage>? Images { get; set; }

        [InverseProperty("Users")]
        [JsonIgnore]
        public virtual IEnumerable<Object> Objects { get; set; }

        [InverseProperty("Users")]
        [JsonIgnore]
        public virtual IEnumerable<Chat> Chats { get; set; }
    }
}
