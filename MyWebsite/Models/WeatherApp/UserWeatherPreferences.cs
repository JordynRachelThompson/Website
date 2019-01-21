using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models.WeatherApp
{
    public class UserWeatherPreferences
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsSubscribed { get; set; } = false;
        public string City { get; set; }
        public bool SevereWeatherDay { get; set; } = false;
        public bool RainyDay { get; set; } = false;
        public bool SnowyDay { get; set; } = false;
        public bool GoodWeatherDay { get; set; } = false;
        public bool BookWeatherDay { get; set; } = false;
        public string AlertsEmail { get; set; }
    }
}
