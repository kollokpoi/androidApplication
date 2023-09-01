namespace DiplomApi.Data.Models
{
    public class ChatUser
    {
        public int Id { get; set; }
        public Chat Chat { get; set; }
        public User User { get; set; }
    }
}
