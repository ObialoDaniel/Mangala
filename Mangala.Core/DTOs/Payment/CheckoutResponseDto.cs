using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.DTOs.Payment
{
    public class CheckoutResponseDto
    {
        public string CheckoutUrl { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }
}
