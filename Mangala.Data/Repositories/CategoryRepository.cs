using Mangala.Core.Interfaces;
using Mangala.Core.Models;

namespace Mangala.Data.Repositories
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public Category Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category? GetById(int id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public bool Delete(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
                return false;
            context.Categories.Remove(category);
            context.SaveChanges();
            return true;
        }
    }
}