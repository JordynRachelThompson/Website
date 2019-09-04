using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebsite.Services
{
    public class EmailMessage
    {
        public string ToAddresses { get; set; } = "";
        public string FromAddresses { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
    }
}
