using Mangala.Core.Models;

namespace Mangala.Core.Interfaces
{
    public interface IRoleRepository
    {
        Role Add(Role role);
        List<Role> GetAll();
        Role? GetByName(string name);
        Role? GetById(int id);

    }
}