using Common.DbModels;
using Common.RequestModel;
using Common.ResponseModel;
using Helper.JWT.httpClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using static JWT.Enums.Common;

namespace Helper.JWT
{
    public class WebUserManager
    {
 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public WebUserManager(IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(LoginStatus loginStatus, ClaimsPrincipal claimsPrincipal)> SignInAsync(string username, string password)
        {
            var requestModel = new LoginRequestModel
            {
                UserName = username,
                Password = password
            };

            // webApi içerisinde istek gönderiyoruz
            var response = await HttpRequestFactory.Post("https://localhost:44330/api/Account/login", requestModel);
            // geriye bize LoginResponseModel gönder,yor 
            var model = response.ContentAsType<LoginResponseModel>();

            var claims = model
                .Claims
                .Select(c => new Claim(c.Type, c.Value));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);
          
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
            return (LoginStatus.Successful, userPrincipal);

        }


   
    }
}
