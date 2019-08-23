using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Models;
using System.Diagnostics;

namespace PortfolioWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UseHomePageLinks = true;

            ViewBag.SpaceApiDescr = "Give Me Space is A RESTful API for retrieving space data and photos from some of NASA’s open APIs. Users may retrieve NASA's Astronomy " +
                "picture of the day, current Rover data, and the most recent Curiosity pictures. Users may also include a date in their call to retrieve the Astronomy " +
                "Picture of the Day and Curiosity pictures (if any) taken on the chosen date.";


            ViewBag.BudgetAppDescr = "The Painless Budget is a user-friendly, responsive budgeting app that " +
                "lets registered users create a monthly budget, view spending insights and analytics, export their budget history to Excel and more.";

            ViewBag.WeatherAppDescr = "Elemet Tracker lets users track weather and view a 5 day, hourly forecast for their selected " +
                "city. Unregistered users can view forecasts and registered users can save their city and manage weather alerts that will " +
                "text or email them when a specific weather condition is present in today's forecast.";

            ViewBag.AboutSecionText = "Hello, I am a full-stack web developer in Tampa and having been working at a local .NET for a little over a year.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
