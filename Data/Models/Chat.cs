using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomApi.Data.Models
{
    public class Chat
    {
        public int Id { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [Required]
        public ChatTypes ChatType { get; set; }

        public virtual List<Message> Message { get; set; }

        // Навигационное свойство для списка пользователей чата
        [InverseProperty("Chats")]
        public virtual IEnumerable<User> Users { get; set; }
    }
    public enum ChatTypes
    {
        main = 0,
        privat = 1,
        group= 2,
    };
}
