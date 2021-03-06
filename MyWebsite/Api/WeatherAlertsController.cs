﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Data.Interfaces;

namespace PortfolioWebsite.Api
{
    [Route("api/[controller]")]
    public class WeatherAlertsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeatherAlertsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("[action]/{newEmail}")]
        public IActionResult SetAlertsEmail(string newEmail)
        {
            _unitOfWork.WeatherRepository.SetAlertsEmail(User.Identity.Name, newEmail);
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetAlertsEmail()
        {
            var email = _unitOfWork.WeatherRepository.ReturnAlertsEmail(User.Identity.Name);
            return Ok(email);
        }
    }
}
