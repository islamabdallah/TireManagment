using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Models;

namespace TireManagment.Services
{
    public class AccountService
    {
        DbContext context;
        UserManager<AppUser> userManger;
        RoleManager<IdentityRole> roleManager;
        SignInManager<AppUser> signInManager;
        public AccountService(DbContext _context, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManger)
        {
            context = _context;
            userManger = _userManger;
            roleManager = _roleManager;
            signInManager = _signInManager;


        }
        public async Task<IdentityResult> Register(RegisterViewModel registerViewModel, string roleNmae)
        {
            AppUser User = new AppUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
                Name = registerViewModel.Name
            };
            var result = await userManger.CreateAsync(User, registerViewModel.Password);
            if (result.Succeeded)
            {
                var user = await userManger.FindByEmailAsync(registerViewModel.Email);

                await userManger.AddToRoleAsync(user, roleNmae);
                //await signInManager.SignInAsync(User, true);


            }
            return result;
        }

        public async Task<AppUser> checkEmailExists(string email)
        {
            return await userManger.FindByEmailAsync(email);
        }

        public AppUser GetUserById(string id)
        {
            var u = context.Users.Where(u => u.Id == id).FirstOrDefault();
            return u;
        }
        public IList<AppUser> GetTireMen()
        {
          var res=  userManger.GetUsersInRoleAsync(Role.TireMan).Result;
            return res;
          //  return context.users.Where(u=>u.ro)
                
        }
        public void createfirstadmin()
        {

        }
             
    }
}
