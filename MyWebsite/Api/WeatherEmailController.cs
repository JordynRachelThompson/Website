using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Data.Interfaces;

namespace PortfolioWebsite.Api
{
    [Route("api/[controller]")]
    public class WeatherEmailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeatherEmailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        public IActionResult GetAlertsEmail()
        {
            var email = _unitOfWork.WeatherRepository.ReturnAlertsEmail(User.Identity.Name);
            return Ok(email);
        }
    }
}
