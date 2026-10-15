using Mangala.Core.Common.Mangala.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mangala.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage message);
    }
}
