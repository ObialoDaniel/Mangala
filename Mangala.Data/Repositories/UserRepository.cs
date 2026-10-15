using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mangala.Data.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public User Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public List<User> GetAll()
        {
            return context.Users.Include(u => u.Role).ToList();
        }

        public User? GetById(int id)
        {
            return context.Users.Include(u => u.Role)
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return context.Users.Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username);   
        }

        public User? GetByEmail(string email)
        {
            return context.Users.Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);
        }
    }
}