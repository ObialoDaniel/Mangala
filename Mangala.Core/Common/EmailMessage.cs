using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mangala.Core.Common
{
    namespace Mangala.Core.Common
    {
        public class EmailMessage
        {
            [Required(ErrorMessage = "Recipient email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string ToEmail { get; set; } = string.Empty;

            [Required(ErrorMessage = "Recipient name is required")]
            public string ToName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Subject is required")]
            public string Subject { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email content is required")]
            public string HtmlContent { get; set; } = string.Empty;
        }
    }
}
