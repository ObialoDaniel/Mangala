using Mangala.Core.Common;
using Mangala.Core.DTOs.Order;
using Mangala.Core.Enum;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Services
{
    public class OrderService(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository) : IOrderService
    {
        public Task<ApiResponse<OrderDto>> CreateOrder(CreateOrderRequest request, int customerId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var orderItems = new List<OrderItem>();
            var dtoItems = new List<OrderItemDto>();
            decimal totalAmount = 0;

            foreach (var itemRequest in request.Items)
            {
                var menuItem = menuItemRepository.GetById(itemRequest.MenuItemId);
                if (menuItem == null)
                {
                    return Task.FromResult(ApiResponse<OrderDto>.Failure
                        ($"Menu item {itemRequest.MenuItemId} not found."));
                }
                
                if (menuItem.AvailableDate != today || !menuItem.IsAvailable)
                {
                    return Task.FromResult(ApiResponse<OrderDto>.Failure
                        ($"Menu item {menuItem.Name} is not available today."));
                }


                if (menuItem.Quantity < itemRequest.Quantity)
                {
                    return Task.FromResult(ApiResponse<OrderDto>.Failure($"Not enough stock for {menuItem.Name}"));
                }

                var subtotal = menuItem.Price * itemRequest.Quantity;
                totalAmount += subtotal;

                orderItems.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = itemRequest.Quantity,
                    Price = menuItem.Price
                });

                dtoItems.Add(new OrderItemDto
                {
                    MenuItemId = menuItem.Id,
                    MenuItemName = menuItem.Name,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = menuItem.Price,
                    Subtotal = subtotal
                });
            }

            var order = new Order
            {
                CustomerId = customerId,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                TotalAmount = totalAmount,
                DateCreated = DateTime.UtcNow,
                Items = orderItems
            };

            var created = orderRepository.Add(order);

            foreach (var itemRequest in request.Items)
            {
                var menuItem = menuItemRepository.GetById(itemRequest.MenuItemId)!;
                menuItemRepository.UpdateQuantity(itemRequest.MenuItemId, menuItem.Quantity - itemRequest.Quantity);
            }

            var dto = new OrderDto
            {
                Id = created.Id,
                CustomerId = created.CustomerId,
                Status = created.Status,
                PaymentStatus = created.PaymentStatus,
                TotalAmount = created.TotalAmount,
                DateCreated = created.DateCreated,
                Items = dtoItems
            };

            return Task.FromResult(ApiResponse<OrderDto>.Success(dto, "Order created successfully."));
        
        }
    }
}
