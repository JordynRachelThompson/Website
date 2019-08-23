using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Data.Interfaces;
using PortfolioWebsite.Models.WeatherApp;
using Newtonsoft.Json;

namespace PortfolioWebsite.Api
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
                //var apiKey = "6a8c9bd2f7fcb773bb364ec94c4551ec";
                var apiKey = "6a8c9bd2f7fcb773bb364ec94c4551ec";
                try
                {
                    var countryCode = "US";
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/forecast?q={city},{countryCode}&appid={apiKey}&units=imperial");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherFiveDayResponse>(stringResult);

                    var infoTime = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");

                    return Ok(new
                    {
                        List = rawWeather.List,
                        City = rawWeather.City.Name,
                        rawWeather.List[0].Weather[0].Icon,
                        Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.ParseExact(rawWeather.List[0].Dt_txt, "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture), infoTime),
                        rawWeather.List[0].Weather[0].Description,
                        rawWeather.List[0].Main.Temp,
                        rawWeather.List[0].Main.temp_max,
                        rawWeather.List[0].Main.temp_min,
                        Clouds = rawWeather.List[0].Clouds.All,
                        rawWeather.List[0].Main.Humidity
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