using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using Mangala.Core.DTOs.MenuItem;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Mangala.Data.Repositories
{
    public class MenuItemRepository(AppDbContext context) : IMenuItemRepository
    {
        public MenuItem Add(MenuItem item)
        {
            context.MenuItems.Add(item);
            context.SaveChanges();
            return item;
        }

        public List<MenuItem> GetByDate(DateOnly date)
        {
            return context.MenuItems
                .Include(m => m.Category)
                .Where(m => m.AvailableDate == date)
                .ToList();
        }

        public MenuItem? GetById(int id)
        {
            return context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefault(m => m.Id == id);
        }

        public bool Delete(int id)
        {
            var item = context.MenuItems.FirstOrDefault(m => m.Id == id);
            if (item is null)
            {
                return false;
            }
            context.MenuItems.Remove(item);
            context.SaveChanges();
            return true;
        }

        public MenuItem? Update(int id, UpdateMenuItemDto request)
        {
            var item = context.MenuItems.FirstOrDefault(m => m.Id == id);
            if (item is null)
            {
                return null;
            }
                
      
            item.Price = request.Price;
            item.Quantity = request.Quantity;
            item.IsAvailable = request.IsAvailable;
        
            context.SaveChanges();
            return item;
        }

        public void UpdateQuantity (int menuItemId, int newQunatity)
        {
            var item = context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
            if (item is null)
            {
                return;
            }
              
            item.Quantity = newQunatity;
            context.SaveChanges();
        }
    }
}
