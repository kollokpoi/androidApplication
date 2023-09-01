using DiplomApi.Data.Models;

namespace DiplomApi.Data.ViewModels
{
    public class UserChatsViewModel
    {
        public Chat MainChat { get; set; }
        public List<Models.Object> ObjectChats { get; set; }
        public List<UserChatViewModel> PrivateChats { get; set; }
    }
}
