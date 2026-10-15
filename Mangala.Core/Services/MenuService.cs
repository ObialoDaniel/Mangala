using Mangala.Core.Common;
using Mangala.Core.DTOs.MenuItem;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Services
{
    public class MenuService(IMenuItemRepository menuItemRepository) : IMenuService
    {
        public ApiResponse<MenuItemDto> CreateMenuItem(CreateMenuItemDto request, int createdByUserId)
        {
            var item = new MenuItem
            {
                Uid = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Quantity = request.Quantity,
                AvailableDate = request.AvailableDate,
                IsAvailable = true,
                DateCreated = DateTime.UtcNow,
                CreatedBy = createdByUserId
            };

            var created = menuItemRepository.Add(item);

            var dto = new MenuItemDto
            {
                Id = created.Id,
                Uid = created.Uid,
                Name = created.Name,
                Description = created.Description,
                CategoryName = created.Category?.Name ?? string.Empty,
                IsAvailable = created.IsAvailable,
                Price = created.Price,
                Quantity = created.Quantity,
                AvailableDate = created.AvailableDate
            };

            return ApiResponse<MenuItemDto>.Success(dto, "Menu item created successfully");
        }

        public ApiResponse<List<MenuItemDto>> GetTodaysMenu()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var items = menuItemRepository.GetByDate(today)
                .Select(m => new MenuItemDto
                {
                    Id = m.Id,
                    Uid = m.Uid,
                    Name = m.Name,
                    Description = m.Description,
                    CategoryName = m.Category?.Name ?? string.Empty,
                    IsAvailable = m.IsAvailable,
                    Price = m.Price,
                    Quantity = m.Quantity,
                    AvailableDate = m.AvailableDate
                }).ToList();

            return ApiResponse<List<MenuItemDto>>.Success(items);
        }

        public ApiResponse<bool> DeleteMenuItem(int id)
        {
            var deleted = menuItemRepository.Delete(id);
            if (!deleted)
                return ApiResponse<bool>.Failure("Menu item not found");

            return ApiResponse<bool>.Success(true, "Menu item deleted successfully");
        }

        public ApiResponse<MenuItemDto> UpdateMenuItem(int id, UpdateMenuItemDto request)
        {
            var updated = menuItemRepository.Update(id, request);
            if (updated is null)
                return ApiResponse<MenuItemDto>.Failure("Menu item not found");

            var dto = new MenuItemDto
            {
                Id = updated.Id,
                Uid = updated.Uid,
                Name = updated.Name,
                Description = updated.Description,
                CategoryName = updated.Category?.Name ?? string.Empty,
                IsAvailable = updated.IsAvailable,
                Price = updated.Price,
                Quantity = updated.Quantity,
                AvailableDate = updated.AvailableDate
            };

            return ApiResponse<MenuItemDto>.Success(dto, "Menu item updated successfully");
        }
    }
}
