using System;
using System.Collections.Generic;
using System.Text;
using Mangala.Core.DTOs.MenuItem;
using Mangala.Core.Models;

namespace Mangala.Core.Interfaces
{
    public interface IMenuItemRepository
    {
        MenuItem Add(MenuItem item);
        List<MenuItem> GetByDate(DateOnly date);
        MenuItem? GetById(int id);
        bool Delete(int id);
        MenuItem? Update(int id, UpdateMenuItemDto request);   
        void UpdateQuantity(int menuItemId, int newQuantity);
    }
}
