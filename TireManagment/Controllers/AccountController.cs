using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
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
            ViewBag.tiremen = accountService.GetTireMen();
            return View();
        }
       
        [HttpGet]
        public IActionResult Admins()
        {
            ViewBag.admins = accountService.GetAdmins();
            return View();
        }
        public IActionResult AdminExcel()
        {
            var admins = accountService.GetAdmins();
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("SystemAdminstrators");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";

                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "UserName";
                worksheet.Cell(currentRow, 5).Value = "PhoneNumber";
             

                foreach (var admin in admins)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = admin.Id;
                    worksheet.Cell(currentRow, 2).Value = admin.Name;
                    worksheet.Cell(currentRow, 3).Value = admin.Email;
                    worksheet.Cell(currentRow, 4).Value = admin.UserName;
                    worksheet.Cell(currentRow, 5).Value = admin.PhoneNumber;
                   

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Admins.xlsx");
                }
            }
        }
        public IActionResult TireMenExcel()
        {
            var tiremen = accountService.GetTireMen();
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("SystemAdminstrators");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";

                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "UserName";
                worksheet.Cell(currentRow, 5).Value = "PhoneNumber";


                foreach (var tireman in tiremen)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = tireman.Id;
                    worksheet.Cell(currentRow, 2).Value = tireman.Name;
                    worksheet.Cell(currentRow, 3).Value = tireman.Email;
                    worksheet.Cell(currentRow, 4).Value = tireman.UserName;
                    worksheet.Cell(currentRow, 5).Value = tireman.PhoneNumber;


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TireMen.xlsx");
                }
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
    
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult RegisterTireman()
        {
            return View("RegisterTierman");
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

                //   return RedirectToAction("Index", "Home");
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

                if (result.Succeeded &&await userManger.IsInRoleAsync(user, Role.Admin)) 
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
