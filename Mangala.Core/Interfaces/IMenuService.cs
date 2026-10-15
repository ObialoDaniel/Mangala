using Mangala.Core.Common;
using Mangala.Core.DTOs.MenuItem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface IMenuService
    {
        ApiResponse<MenuItemDto> CreateMenuItem(CreateMenuItemDto request, int createdByUserId);
        ApiResponse<List<MenuItemDto>> GetTodaysMenu();
        ApiResponse<bool> DeleteMenuItem(int id);
        ApiResponse<MenuItemDto> UpdateMenuItem(int id, UpdateMenuItemDto request);
    }
}