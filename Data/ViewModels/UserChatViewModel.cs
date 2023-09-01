using DiplomApi.Data.Models;

namespace DiplomApi.Data.ViewModels
{
    public class UserChatViewModel
    {
		private Guid userId;
		public UserChatViewModel(Guid id) 
		{
			userId= id;
		}
        public User User { get; private set; }
		private Chat Chat;

		public Chat PrivateChat
		{
			get { return Chat; }
			set {
				Chat = value;
				User = value.Users.First(x => x.Id != userId);
				Chat.Name= $"{User.FirstName} {User.LastName}";
			}
		}
	}
}
