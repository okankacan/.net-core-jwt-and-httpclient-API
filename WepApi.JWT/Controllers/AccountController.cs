using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.DbModels;
using Common.RequestModel;
using Common.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static JWT.Enums.Common;

namespace WepApi.JWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        {

            try
            {


                var user = await _userManager.FindByNameAsync(model.UserName);
 

                if (user == null || await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    return BadRequest("NotFound");
                }

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
               

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("251a905e08844317a5ea47ab0f6ea2dd"));
                var token = new JwtSecurityToken(
                    issuer: "Fiver.Security.Bearer",
                    audience: "Fiver.Security.Bearer",
                    expires: DateTime.UtcNow.AddHours(2),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
     
                var claimResponse = claims.Select(c => new ClaimResponseModel
                {
                    Type = c.Type,
                    Value = c.Value
                });
                return Ok(new LoginResponseModel
                {
                    Claims = claimResponse,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ValidTo = token.ValidTo,
                    Status = LoginStatus.Successful
                });
            }
            catch (Exception ex)
            {
                return BadRequest( new LoginResponseModel());
            }

        }
    }
}