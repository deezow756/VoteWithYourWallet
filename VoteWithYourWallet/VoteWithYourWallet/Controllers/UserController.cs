using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoteWithYourWallet.Models;
using VoteWithYourWallet.Models.ViewModels;

namespace VoteWithYourWallet.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("User/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("User/Login")]
        public async Task<IActionResult> Login(Login model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Redirect("/");
                    }

                    ModelState.AddModelError("", "Username or Password incorrect");
                }
            }
            return View();
        }

        [HttpGet]
        [Route("User/CheckLogin")]
        public async Task<JsonResult> CheckLogin(string email, string password)
        {
            JsonResponseModel model = null;
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (await userManager.CheckPasswordAsync(user, password))
                {
                    model = new JsonResponseModel() { status = true, msg = "Successfully Logged In" };
                    return new JsonResult(model);
                }
                else
                {
                    model = new JsonResponseModel() { status = false, msg = "Wrong Password" };
                    return new JsonResult(model);
                }
            }
            else
            {
                model = new JsonResponseModel() { status = false, msg = "No Account Registered With Email: " + email };
                return new JsonResult(model);
            }
        }

        [HttpGet]
        [Route("User/CheckEmail")]
        public async Task<JsonResult> CheckEmail(string email)
        {
            JsonResponseModel model = null;
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                model = new JsonResponseModel() { status = true };
                return new JsonResult(model);
            }
            else
            {
                model = new JsonResponseModel() { status = false };
                return new JsonResult(model);
            }
        }

        [HttpGet]
        [Route("User/CheckUsername")]
        public async Task<JsonResult> CheckUsername(string username)
        {
            JsonResponseModel model = null;
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                model = new JsonResponseModel() { status = true, msg = "Username already taken" };
                return new JsonResult(model);
            }
            else
            {
                model = new JsonResponseModel() { status = false, msg = "" };
                return new JsonResult(model);
            }
        }

        [HttpGet]
        [Route("User/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("User/Register")]
        public async Task<IActionResult> Register(Register model)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var role = await roleManager.FindByNameAsync("Member");

                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                    await signInManager.SignInAsync(user, false);
                    return Redirect("/");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
