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
            ViewBag.City = false;
            var userCity = _unitOfWork.WeatherRepository.returnUserCity(User.Identity.Name);
            if (userCity != "")
                ViewBag.City = userCity;

           return View();
        }
    }
}