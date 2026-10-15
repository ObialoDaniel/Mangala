using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using System.Data;

namespace Mangala.Data.Repositories
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        public Role Add(Role role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
            return role;
        }

        public List<Role> GetAll()
        {
            return context.Roles.ToList();
        }
        public Role? GetByName(string name)
        {
            return context.Roles.FirstOrDefault(r => r.Name == name);
        }
        public Role? GetById(int id)
        {
            return context.Roles.FirstOrDefault(x => x.Id == id);
        }
    }
}