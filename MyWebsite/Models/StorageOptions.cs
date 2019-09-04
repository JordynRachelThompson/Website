using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebsite.Models
{
    public class StorageOptions
    {
        public string ReCaptchaKey { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpUsername { get; set; }
    }
}
