using Mangala.Core.Common;
using Mangala.Core.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface ICategoryService
    {
        ApiResponse<CategoryDto> CreateCategory(CreateCategoryDto request, int createdByUserId);
        ApiResponse<List<CategoryDto>> GetAllCategories();
        ApiResponse<bool> DeleteCategory(int id);
    }
}
