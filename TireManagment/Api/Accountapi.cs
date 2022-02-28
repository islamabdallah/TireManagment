using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Models;
using TireManagment.Services;

namespace TireManagment.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Accountapi : ControllerBase
    {
        AccountService accountService;
        SignInManager<AppUser> signInManger;
        UserManager<AppUser> userManger;
        public Accountapi(AccountService _accountService, SignInManager<AppUser> _signInManger, UserManager<AppUser> _UserManager)
        {
            accountService = _accountService;
            signInManger = _signInManger;
            userManger = _UserManager;
        }
     [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            var result = await accountService.Register(registerViewModel, Role.TireMan);

            if (result.Succeeded)
            {

                return Ok(result);
            }
            return BadRequest(result.Errors);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            var user = await userManger.FindByEmailAsync(loginViewModel.email);
            if (user != null)
            {
                var result = await signInManger.PasswordSignInAsync(user.UserName, loginViewModel.password, loginViewModel.RememberMe, true);

                if (result.Succeeded)
                {


                    
                    return Ok(user);
                }
                return BadRequest("Password Not Correct");
            }
            return BadRequest("Email Not Exist");
             

        }
        
    

    }
}
