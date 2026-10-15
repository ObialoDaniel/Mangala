using Mangala.API.Extensions;
using Mangala.Core.DTOs.Order;
using Mangala.Core.Interfaces;
using Mangala.Core.Models;
using Mangala.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mangala.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class OrderController(IOrderService orderService, IPaymentService paymentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var customerId = User.GetUserId();
            var response = await orderService.CreateOrder(request, customerId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("{id}/checkout")]
        public async Task<IActionResult> Checkout(int id)
        {
            var customerId = User.GetUserId();
            var response = await paymentService.CreateCheckout(id, customerId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}/payment-status")]
        public async Task<IActionResult> PaymentStatus(int id)
        {
            var customerId = User.GetUserId();
            var response = await paymentService.CheckPaymentStatus(id, customerId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}