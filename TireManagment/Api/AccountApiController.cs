using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Models;
using TireManagment.Services;

namespace TireManagment.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        AccountService AccountService;
        SignInManager<AppUser> SignInManger;
        UserManager<AppUser> UserManger;

        public AccountApiController(AccountService _accountService, SignInManager<AppUser> _signInManger, UserManager<AppUser> _UserManager)
        {
            AccountService = _accountService;
            SignInManger = _signInManger;
            UserManger = _UserManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var result = await AccountService.Register(registerViewModel, Role.TireMan);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var _user = await UserManger.FindByEmailAsync(loginViewModel.email);
            if (_user != null)
            {
                var _result = await SignInManger.PasswordSignInAsync(_user.UserName, loginViewModel.password, loginViewModel.RememberMe, true);

                if (_result.Succeeded)
                {
                    return Ok(new { Flag = true, Message = "Done", Data = _result });
                }
                return BadRequest(new { Flag = false, Message = "Error , Password Not Correct", Data = 0 }); 
            }
            return BadRequest(new { Flag = false, Message = "Error , Email Not Exist", Data = 0 });
        }
    }
}
