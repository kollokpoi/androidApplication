using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiplomApi.Data.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        [Required]
        public byte[]? ImageData { get; set; }
        [Required]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
