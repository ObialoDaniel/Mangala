using Mangala.Core.Common;
using Mangala.Core.DTOs.Payment;
using Mangala.Core.Interfaces;
using Mangala.Core.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Mangala.Core.Services
{
    public class PaymentService(HttpClient httpClient, BachsSettings bachsSettings, IOrderRepository orderRepository) : IPaymentService
    {
        public async Task<ApiResponse<CheckoutResponseDto>> CreateCheckout (int orderId, int custonerId)
        {

            var order = orderRepository.GetById(orderId);

            if (order is null || order.CustomerId != custonerId)
            {
                return ApiResponse<CheckoutResponseDto>.Failure("Order not found");

            }
            
            if (order.PaymentStatus == Enum.PaymentStatus.Paid)
            {
                return ApiResponse<CheckoutResponseDto>.Failure("Order is already paid");
            }

            var productPayload = new
            {
                name = $"Order #{order.Id}",
                price = new { amount = order.TotalAmount.ToString("F2"), currency = "NGN" }
            };

            var productResponse = await Send("/v1/products", productPayload);
            using var productDoc = JsonDocument.Parse(productResponse);
            var productId = productDoc.RootElement.GetProperty("id").GetString();

            var checkoutPayload = new
            {
                customer = new { email = order.User.Email, name = order.User.FullName },
                product_cart = new[] { new { product_id = productId, quantity = 1 } },
                reference = order.Id.ToString()
            };

            var checkoutResponse = await Send("/v1/checkout-sessions", checkoutPayload);
            using var checkoutDoc = JsonDocument.Parse(checkoutResponse);

            var dto = new CheckoutResponseDto
            {
                CheckoutUrl = checkoutDoc.RootElement.GetProperty("checkout_url").GetString()!,
                Reference = checkoutDoc.RootElement.GetProperty("reference").GetString()!
            };

            return ApiResponse<CheckoutResponseDto>.Success(dto, "Checkout session created");
        }

        public async Task<ApiResponse<string>> CheckPaymentStatus(int orderId, int customerId)
        {
            var order = orderRepository.GetById(orderId);

            if (order is null || order.CustomerId != customerId)
                return ApiResponse<string>.Failure("Order not found");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{bachsSettings.BaseUrl}/v1/payments");
            request.Headers.Add("Authorization", $"Bearer {bachsSettings.ApiKey}");

            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(body);
            foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
            {
                if (item.GetProperty("reference").GetString() == order.Id.ToString())
                    return ApiResponse<string>.Success(item.GetProperty("status").GetString()!, "Status retrieved");
            }

            return ApiResponse<string>.Success("no_payment_found", "No payment found for this order yet");
        }
        private async Task<string> Send(string path, object payload)
        {
            var json = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{bachsSettings.BaseUrl}{path}")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {bachsSettings.ApiKey}");

            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return body;
        }
    }
}