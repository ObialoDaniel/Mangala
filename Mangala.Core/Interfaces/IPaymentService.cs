using Mangala.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Mangala.Core.DTOs.Payment;

namespace Mangala.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse<CheckoutResponseDto>> CreateCheckout(int orderId, int customerId);
        Task<ApiResponse<string>> CheckPaymentStatus(int orderId, int customerId);
    }
}