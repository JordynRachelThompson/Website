using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebsite.Data.Interfaces
{
    public interface IWeatherRepository
    {
        bool DoesUserHaveSavedCity(string user);
        void CreateUserProfile(string city, string user);
        void SetUserSelectedCity(string city, string user);
        string ReturnUserCity(string user);
        string ReturnAlertsEmail(string user);
        void SetAlertsEmail(string user, string newEmail);
    }
}
