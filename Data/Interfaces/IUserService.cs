using DiplomApi.Data.Models;

namespace DiplomApi.Data.Interfaces
{
    public interface IUserService
    {
        User GetUserByUsernameAndPassword(string username, string password);
        int GetUserIdByCookie(string authCookie);
        User GetUserById(Guid userId);
    }
}
