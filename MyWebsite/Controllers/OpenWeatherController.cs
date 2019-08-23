using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Data.Interfaces;

namespace PortfolioWebsite.Controllers
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

           return View();
        }
    }
}