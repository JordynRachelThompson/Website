using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Data.Interfaces
{
    public interface IWeatherRepository
    {
        bool DoesUserHaveSavedCity(string user);
        void createUserProfile(string city, string user);
        void SetUserSelectedCity(string city, string user);
        string returnUserCity(string user);
    }
}
