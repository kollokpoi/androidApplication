using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DiplomApi.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        [JsonIgnore]
        public virtual Chat Chat { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public string MessageText { get; set; }

        public virtual List<MessageData> MessageData { get; set; }
    }
}
