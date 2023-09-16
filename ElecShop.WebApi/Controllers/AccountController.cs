using ElecShop.Core.DTOs.Account;
using ElecShop.WebApi.Core.DTOs.Account;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Services.Implementation;
using ElecShop.WebApi.Core.Utilities.Common;
using ElecShop.WebApi.Core.Utilities.Extensions.Identity;
using ElecShop.WebApi.DataLayer.Entities.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElecShop.WebApi.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Constructor

        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion Constructor

        #region Register

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto register)
        {

            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUser(register);
                switch (result)
                {
                    case RegisterUserResult.EmailExists:
                        return JsonResponseStatus.Error(new { status = "EmailExists" });
                }
            }

            return JsonResponseStatus.Success();

        }
        #endregion Register

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

        #region Sign out

        [HttpPost("Sign-out")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return JsonResponseStatus.Success();

            }
            return JsonResponseStatus.Error();

        }

        #endregion Sign out

        #region Check User Authenticated

        [HttpPost("Check-Auth")]
        public async Task<IActionResult> CheckUserAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                var user = await _userService.GetUserByUserId(userId);
                return JsonResponseStatus.Success(new
                {
                    userId = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    address = user.Address
                }
                    );
            }
            return JsonResponseStatus.Error();

        }
        #endregion Check User Authenticated

        #region Activate User Account

        [HttpGet("activate-account/{id}")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _userService.GetUserByEmailActiveCode(id);
            if (user != null)
            {
                _userService.ActivateUser(user);
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.NotFound();
        }
        #endregion Activate User Account

        #region Edit user

        [HttpPost("edit-user")]
        public async Task<IActionResult> EditUser(EditUserDto editUser)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                await _userService.EditUserInfo(editUser, userId);
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.UnAuthorized();
        }

        #endregion Edit user
    }
}
