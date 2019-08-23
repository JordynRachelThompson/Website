using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Data.Interfaces;
using PortfolioWebsite.Models.WeatherApp;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace PortfolioWebsite.Api
{
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeatherController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("[action]/{city}")]
        public async Task<IActionResult> City(string city)
        {
            using (var client = new HttpClient())
            {
                var apiKey = "6a8c9bd2f7fcb773bb364ec94c4551ec";
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={apiKey}&units=imperial");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);

                    var user = User.Identity.Name;
                    var userAlreadyHasSavedCity = user != null && _unitOfWork.WeatherRepository.DoesUserHaveSavedCity(user);

                    if(userAlreadyHasSavedCity)
                        _unitOfWork.WeatherRepository.SetUserSelectedCity(city, user);

                    if (!userAlreadyHasSavedCity && user != null)
                        _unitOfWork.WeatherRepository.CreateUserProfile(city, user);

                    _unitOfWork.Complete();
                    
                    return Ok(new
                    {
                        City = rawWeather.Name,
                        rawWeather.Main.Temp,
                        Icon = rawWeather.Weather.Select(x => x.Icon),
                        Description = rawWeather.Weather.Select(x => x.Description)
                    });
                    
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                };
            }
        }
    }
}

