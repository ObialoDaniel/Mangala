using Mangala.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mangala.Core.Settings;
using System.Text;
using System.Security.Cryptography;

namespace Mangala.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController(IOrderRepository orderRepository, BachsSettings bachsSettings) : ControllerBase
    {
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var timestamp = Request.Headers["X-Bachs-Timestamp"].ToString();
            var signature = Request.Headers["X-Bachs-Signature"].ToString();

            if (!VerifySignature(timestamp, rawBody, signature, bachsSettings.WebHookSigningSecret))
                return Unauthorized();

            using var doc = System.Text.Json.JsonDocument.Parse(rawBody);
            var eventType = doc.RootElement.GetProperty("type").GetString();
            var reference = doc.RootElement.GetProperty("data").GetProperty("reference").GetString();

            if (reference is null || !int.TryParse(reference, out var orderId))
                return Ok();

            var order = orderRepository.GetById(orderId);
            if (order is null)
                return Ok();

            if (eventType == "checkout.completed" || eventType == "collection.succeeded")
            {
                order.PaymentStatus = Mangala.Core.Enum.PaymentStatus.Paid;
                orderRepository.Update(order);
            }
            else if (eventType == "checkout.expired" || eventType == "collection.failed")
            {
                order.PaymentStatus = Mangala.Core.Enum.PaymentStatus.Failed;
                orderRepository.Update(order);
            }

            return Ok();
        }

        private bool VerifySignature(string timestamp, string rawBody, string signatureHeader, string secret)
        {
            if (string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(signatureHeader))
                {
                return false;
            }

            var signedMessage = $"{timestamp}.{rawBody}";
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(signedMessage);

            using var hmac = new HMACSHA256(secretBytes);
            var computedHash = hmac.ComputeHash(messageBytes);
            var computedSignature = Convert.ToHexString(computedHash).ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(signatureHeader.ToLowerInvariant())
            );
        }
    }
}
