using Mangala.Core.Common;
using Mangala.Core.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderDto>> CreateOrder(
        CreateOrderRequest request,
        int customerId);
    }
}
