using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PortfolioWebsite.Services
{
    public interface IEmailService
    {
        bool SendContactMessage(EmailMessage emailMessage);
    }
}
