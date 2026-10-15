using Mangala.Core.Models;

namespace Mangala.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Category Add(Category category);
        List<Category> GetAll();
        Category? GetById(int id);
        bool Delete(int id);
    }
}
