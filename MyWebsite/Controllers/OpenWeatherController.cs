using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Data.Interfaces;

namespace MyWebsite.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OpenWeatherController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var userCity = _unitOfWork.WeatherRepository.ReturnUserCity(User.Identity.Name);

            ViewBag.City = userCity ?? "False";

            var alertsEmail = _unitOfWork.WeatherRepository.ReturnAlertsEmail(User.Identity.Name);

            ViewBag.AlertsEmail = alertsEmail ?? ""; 

           return View();
        }
    }
}