using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models;
using System.Diagnostics;

namespace MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UseHomePageLinks = true;

            ViewBag.BudgetAppDescr = "The Painless Budget is a user-friendly, responsive budgeting app that " +
                "lets registered users create a monthly budget, view spending insights and analytics, export their budget history to Excel and more.";      

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
