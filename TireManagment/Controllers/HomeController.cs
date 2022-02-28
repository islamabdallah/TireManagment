using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Models;

namespace TireManagment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        SignInManager<AppUser> signInManger;
        public HomeController(ILogger<HomeController> logger,SignInManager<AppUser>_signInManger)
        {
            _logger = logger;
            signInManger = _signInManger;
        }

        public IActionResult Index()
        {
           // var ser=User;
            if(User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult table()

        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
