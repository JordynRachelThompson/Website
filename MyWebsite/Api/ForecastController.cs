using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.WeatherApp;
using Newtonsoft.Json;

namespace MyWebsite.Api
{
    [Route("api/[controller]")]
    public class ForecastController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ForecastController(IUnitOfWork unitOfWork)
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
                    var countryCode = "US";
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/forecast?q={city},{countryCode}&appid={apiKey}&units=imperial");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherFiveDayResponse>(stringResult);

                    return Ok(new
                    {
                        City = rawWeather.City.Name,
                        rawWeather.List[0].Weather[0].Icon,
                        Date = rawWeather.List[0].Dt_txt,
                        rawWeather.List[0].Weather[0].Description,
                        rawWeather.List[0].Main.Temp,
                        rawWeather.List[0].Main.temp_max,
                        rawWeather.List[0].Main.temp_min,
                        Clouds = rawWeather.List[0].Clouds.All,
                        rawWeather.List[0].Main.Humidity,
                        List = rawWeather.List

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