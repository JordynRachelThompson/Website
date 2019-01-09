using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.WeatherApp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebsite.Api
{
    [Route("api/[controller]")]
    public class AddWeatherCityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddWeatherCityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult SetWeatherCity(Weather weatherData)
        {

            return Ok();
        }
    }
}
