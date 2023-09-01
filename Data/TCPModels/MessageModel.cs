using DiplomApi.Data.Models;

namespace DiplomApi.Data.TCPModels
{
    public class MessageModel
    {
        public int ChatId { get; set; }
        public Guid UserId { get; set; }

        public Message Message { get; set; }
    }
}
