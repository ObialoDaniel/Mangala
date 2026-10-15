using Mangala.Core.Models;

namespace Mangala.Core.Interfaces
{
    public interface IUserRepository
    {
        User Add(User user);
        List<User> GetAll();
        User? GetById(int id);
        User? GetByUsername(string username);
        User? GetByEmail(string email);
    }
}
