using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PortfolioWebsite.Models;
using PortfolioWebsite.Services;
using System.Diagnostics;

namespace PortfolioWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<StorageOptions> _config;        
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IConfiguration _configuration;

        public HomeController(IEmailConfiguration emailConfiguration, IOptions<StorageOptions> config)
        {
            _emailConfiguration = emailConfiguration;
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.UseHomePageLinks = true;

            ViewBag.ReCaptchaKey = _config.Value.ReCaptchaKey;

            ViewBag.SpaceApiDescr = "Give Me Space is A RESTful API for retrieving space data and pictures from NASA’s open APIs. Users may retrieve NASA's current (APOD) Astronomy " +
                "Picture of the Day, Curiosity rover data, and a collection of Curiosity's most recently captured pictures. Users may also supply a date to retrieve the APOD " +
                "for a certain date and pictures captured by Curiosity (if any) for the inputted date.";


            ViewBag.BudgetAppDescr = "The Painless Budget is a user-friendly, responsive budgeting app that " +
                "lets registered users create a monthly budget, view spending insights and analytics, export their budget history to Excel and more.";

            ViewBag.WeatherAppDescr = "Elemet Tracker lets users track weather and view a 5 day, hourly forecast for their selected " +
                "city. Unregistered users can view forecasts and registered users can save their city and manage weather alerts that will " +
                "email alert them when a specific weather condition is present in today's forecast.";

            return View();
        }

        [HttpPost]
        public IActionResult Index(EmailMessage emailMessage)
        {
            var sendEmail = new EmailService(_emailConfiguration, _config);

           

            if (sendEmail.SendContactMessage(emailMessage))
            {
                ViewBag.EmailMessage = "Your message was sent. Thank you!";
            }
            else
            {
                ViewBag.EmailMessage = "The was an error sending your message.";
            }

            ViewBag.UseHomePageLinks = true;

            ViewBag.ReCaptchaKey = _config.Value.ReCaptchaKey;

            ViewBag.SpaceApiDescr = "Give Me Space is A RESTful API for retrieving space data and pictures from NASA’s open APIs. Users may retrieve NASA's current (APOD) Astronomy " +
                "Picture of the Day, Curiosity rover data, and a collection of Curiosity's most recently captured pictures. Users may also supply a date to retrieve the APOD " +
                "for a certain date and pictures captured by Curiosity (if any) for the inputted date.";


            ViewBag.BudgetAppDescr = "The Painless Budget is a user-friendly, responsive budgeting app that " +
                "lets registered users create a monthly budget, view spending insights and analytics, export their budget history to Excel and more.";

            ViewBag.WeatherAppDescr = "Elemet Tracker lets users track weather and view a 5 day, hourly forecast for their selected " +
                "city. Unregistered users can view forecasts and registered users can save their city and manage weather alerts that will " +
                "email alert them when a specific weather condition is present in today's forecast.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
