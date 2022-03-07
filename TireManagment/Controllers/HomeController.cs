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
        UserManager<AppUser> userManager;
        public HomeController(UserManager<AppUser> _userManager, ILogger<HomeController> logger,SignInManager<AppUser>_signInManger)
        {
            _logger = logger;
            signInManger = _signInManger;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            //var ser=User;
            var user = userManager.GetUserAsync(User).Result;
           
            if (user!=null)
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
