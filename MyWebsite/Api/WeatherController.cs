using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models.WeatherApp;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace MyWebsite.Api
{
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
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
                    return Ok(new
                    {
                        City = rawWeather.Name,
                        rawWeather.Main.Temp,
                        rawWeather.Main.Humidity,
                        rawWeather.Main.temp_max,
                        rawWeather.Main.temp_min,
                        Icon = rawWeather.Weather.Select(x => x.Icon)
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

