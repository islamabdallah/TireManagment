using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Models;
using TireManagment.Services;

namespace TireManagment.Controllers
{
    public class AccountController : Controller
    {
        AccountService accountService;
        SignInManager<AppUser> signInManger;
        UserManager<AppUser> userManger;
        public AccountController(AccountService _accountService, SignInManager<AppUser> _signInManger, UserManager<AppUser> _UserManager)
        {
            accountService = _accountService;
            signInManger = _signInManger;
            userManger = _UserManager;
        }
        [HttpGet]
        public IActionResult Tiremen()
        {
            return View(accountService.GetTireMen());
        }
        //[Authorize(Roles = Role.Admin)]
        public IActionResult Register()
        {
            return View("Register");
        }
    
        public IActionResult Login()
        {
            return View();
        }
        //[Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel registerViewModel)
        {
          return await Register(registerViewModel, Role.Admin);
        }
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RegisterTireMan(RegisterViewModel registerViewModel)
        {
          return await  Register(registerViewModel, Role.TireMan);
        }
        [Authorize(Roles = Role.Admin)]
        private async Task<IActionResult> Register(RegisterViewModel registerViewModel, string roleName)
        {

            var result = await accountService.Register(registerViewModel, roleName);

            if (result.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty,result.ToString());
            return View("Register", registerViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            var user = await userManger.FindByEmailAsync(loginViewModel.email);
            if (user != null)
            {
                var result = await signInManger.PasswordSignInAsync(user.UserName, loginViewModel.password, loginViewModel.RememberMe, true);
                
                if (result.Succeeded && User.IsInRole(Role.Admin))
                {


               
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Email or Password Not Correct");



            return View(loginViewModel);

        }
        public IActionResult LogOut()
        {
            signInManger.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult setting()
        {
           AppUser user= userManger.GetUserAsync(User).Result;
            return View(user);
        }
        public IActionResult UpdateProfile(AppUser uuser)
        {
            AppUser user = userManger.GetUserAsync(User).Result;
            user.Email = uuser.Email;
            user.Name = uuser.Name;
         var result=   userManger.UpdateAsync(user);
            return RedirectToAction("Index", "Home");
        }

    }
}
