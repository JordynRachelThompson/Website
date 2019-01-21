using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.WeatherApp;

namespace MyWebsite.Data.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly ApplicationDbContext _context;

        public WeatherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool DoesUserHaveSavedCity(string user)
        {
            var hasCity = false;
            var userData = _context.Weather.Where(x => x.Email == user).Select(x => x.City).FirstOrDefault();
            if (userData != null)
                hasCity = true;
            return hasCity;
        }

        public void CreateUserProfile(string city, string user)
        {
            var userInfo = new UserWeatherPreferences
            {
                City = city,
                Email = user,
                AlertsEmail = user
            };

            _context.Weather.Add(userInfo);
        }

        public void SetUserSelectedCity(string city, string user)
        {
            var userCity = _context.Weather.Where(x => x.Email == user).Select(x => x.City).FirstOrDefault();
            if (userCity == city) return;
            var userInfo = _context.Weather.Where(x => x.Email == user).Select(x => x).FirstOrDefault(); //fix this
            if (userInfo == null) return;
            userInfo.City = city;
            _context.Entry(userInfo).State = EntityState.Modified;
        }

        public string ReturnUserCity(string user)
        {
            return _context.Weather.Where(x => x.Email == user).Select(x => x.City).FirstOrDefault();
        }

        public string ReturnAlertsEmail(string user)
        {
            return _context.Weather.Where(x => x.Email == user).Select(x => x.AlertsEmail).FirstOrDefault();
        }

        public void SetAlertsEmail(string user, string newEmail)
        {
            var account = _context.Weather.Where(x => x.Email == user).Select(x => x).FirstOrDefault();
            if (account == null || account.AlertsEmail == newEmail) return;

            account.AlertsEmail = newEmail;
            _context.Entry(account).State = EntityState.Modified;
        }
    }
}
