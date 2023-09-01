using DiplomApi.Data.Database;
using DiplomApi.Data.Interfaces;
using DiplomApi.Data.Models;

namespace DiplomApi.Services
{
    public class UserService : IUserService
    {
        public User GetUserById(Guid userId)
        {
            DatabaseContext context = Program.context;
            return context.Users.FirstOrDefault(x => x.Id == userId);
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            DatabaseContext context = Program.context;
            return context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public int GetUserIdByCookie(string authCookie)
        {
            throw new NotImplementedException();
        }
    }
}
