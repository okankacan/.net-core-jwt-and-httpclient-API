using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DbModels;
using Common.ViewModel;
using Helper.JWT;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static JWT.Enums.Common;

namespace JWT.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
         private readonly WebUserManager _webUserManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, WebUserManager webUserManager)
        {
            _signInManager = signInManager;
            _webUserManager = webUserManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (model.ReturnUrl == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(model.ReturnUrl);
            }
            var result = await _webUserManager.SignInAsync(model.UserName, model.Password);
            if(result.loginStatus== LoginStatus.Successful)
            {
                
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
} 