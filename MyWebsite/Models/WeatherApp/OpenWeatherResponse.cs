using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyWebsite.Models.WeatherApp
{
    public class OpenWeatherResponse
    {
        public string Name { get; set; }

        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }
    }

    public class OpenWeatherFiveDayResponse
    {
        public IEnumerable<WeatherFiveDay> Weather { get; set; }
        public Main Main { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
        public List[] List { get; set; }
        public City City { get; set; }
        public RootObject RootObject { get; set; }
    }

    [DataContract(Name="Weather")]
    public class WeatherFiveDay
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Main { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Icon { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Main
    {
        public string Temp { get; set; }
        public string Pressure { get; set; }
        public int Humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public double Deg { get; set; }
    }

    public class Sys
    {
        public string Pod { get; set; }
    }

    public class List
    {
        public int Dt { get; set; }
        public Main Main { get; set; }
        public WeatherFiveDay[] Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
        public string Dt_txt { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public Coord coord { get; set; }
        public string Country { get; set; }
        public string Cod { get; set; }
        public double Message { get; set; }
        public int Cnt { get; set; }
        public List<List> List { get; set; }
    }

    public class RootObject
    {
        public City City { get; set; }
    }
}
