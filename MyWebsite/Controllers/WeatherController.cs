using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyWebsite.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
           return View();
        }

        public IActionResult ManageWeatherAccount()
        {
            return View();
        }

        public IActionResult ManageWeatherAlerts()
        {
            return View();
        }
    }
}