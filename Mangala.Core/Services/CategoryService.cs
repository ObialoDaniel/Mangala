using Mangala.Core.Common;
using Mangala.Core.DTOs.Category;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Services
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public ApiResponse<CategoryDto> CreateCategory(CreateCategoryDto request, int createdByUserId)
        {
            var category = new Category
            {
                Name = request.Name,
                DateCreated = DateTime.UtcNow,
                CreatedBy = createdByUserId
            };

            var created = categoryRepository.Add(category);

            var dto = new CategoryDto
            {
                Id = created.Id,
                Name = created.Name,
                DateCreated = created.DateCreated
            };
            return ApiResponse<CategoryDto>.Success(dto, "Category created successfully");
        }

        public ApiResponse<List<CategoryDto>> GetAllCategories()
        {
            var categories = categoryRepository.GetAll()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    DateCreated = c.DateCreated
                }).ToList();
            return ApiResponse<List<CategoryDto>>.Success(categories);
        }

        public ApiResponse<bool> DeleteCategory(int id)
        {
            var deleted = categoryRepository.Delete(id);
            if (!deleted)
                return ApiResponse<bool>.Failure("Category not found");
            return ApiResponse<bool>.Success(true, "Category deleted successfully");
        }
    }
}
