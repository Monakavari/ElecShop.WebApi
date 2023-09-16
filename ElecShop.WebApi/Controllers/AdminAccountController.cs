using ElecShop.WebApi.Core.DTOs.Account;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Utilities.Common;
using ElecShop.WebApi.Core.Utilities.Extensions.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElecShop.WebApi.Controllers
{
    public class AdminAccountController : SiteBaseController
    {
        #region Constructor

        private readonly IUserService _userService;
        public AdminAccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion Constructor

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(login, true);

                switch (result)
                {
                    case LoginUserResult.IncorrectData:
                        return JsonResponseStatus.NotFound();

                    case LoginUserResult.NotActivated:
                        return JsonResponseStatus.Error(new { message = "حساب کاربری شما فعال نشده است" });

                    case LoginUserResult.NotAdmin:
                        return JsonResponseStatus.Error(new { message = "شما به این بخش دسترسی ندارید" });

                    case LoginUserResult.Success:
                        var user = await _userService.GetUserByEmail(login.Email);
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularEshopJwtBearer"));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokenOptions = new JwtSecurityToken(
                            issuer: "https://localhost:44381",
                            claims: new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Email),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            },
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                        return JsonResponseStatus.Success(new { token = tokenString, expireTime = 30, firstName = user.FirstName, lastName = user.LastName, userId = user.Id });
                }
            }

            return JsonResponseStatus.Error();
        }
        #endregion Login

        #region Check Admin Authenticated

        [HttpPost("Check-admin-Auth")]
        public async Task<IActionResult> CheckAdminAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                var user = await _userService.GetUserByUserId(userId);

                if (await _userService.IsUserAdmin(userId))
                {
                    return JsonResponseStatus.Success(new
                    {
                        userId = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        address = user.Address
                    });
                }
            }
            return JsonResponseStatus.Error();

        }
        #endregion Check Admin Authenticated
    }
}
