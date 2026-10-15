using Mangala.Core.Common.Mangala.Core.Common;
using Mangala.Core.Interfaces;
using Mangala.Core.Settings;
using System.Text;
using System.Text.Json;

namespace Mangala.Core.Services
{
    public class EmailService(HttpClient httpClient, BrevoSettings brevoSettings) : IEmailService
    {
        public async Task SendAsync(EmailMessage message)
        {
            
            var payload = new
            {
                sender = new { email = brevoSettings.SenderEmail, name = brevoSettings.SenderName },
                to = new[] { new { email = string.IsNullOrWhiteSpace(message.ToEmail) ? string.Empty : message.ToEmail,
                    name = string.IsNullOrWhiteSpace(message.ToName) ? string.Empty : message.ToName } },
                subject = string.IsNullOrWhiteSpace(message.Subject) ? string.Empty : message.Subject,
                htmlContent = string.IsNullOrWhiteSpace(message.HtmlContent) ? string.Empty : message.HtmlContent
            };

            var json = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("api-key", brevoSettings.ApiKey);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}