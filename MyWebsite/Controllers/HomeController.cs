using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models;
using System.Diagnostics;

namespace MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UseHomePageLinks = true;

            ViewBag.BudgetAppDescr = "The Painless Budget is a user-friendly, responsive budgeting app that " +
                "lets registered users create a monthly budget, view spending insights and analytics, export their budget history to Excel and more.";

            ViewBag.WeatherAppDescr = "Open Weather Elemet Alerts lets users track weather via Open WeatherMap's API and view a 5 day, hourly forecast for their selected " +
                "city. Unregistered users can also view forecasts. Once registered, users can set their city and manage element alerts that will " +
                "alert them when a particular weather condition is present in today's forecast.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
