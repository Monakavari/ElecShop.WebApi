using ElecShop.Core.DTOs.Account;
using ElecShop.WebApi.Core.DTOs.Account;
using ElecShop.WebApi.DataLayer.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.Core.Services.Contracts
{
    public interface IUserService : IDisposable
    {
        Task<List<User>> GetAllUsers();
        Task<RegisterUserResult> RegisterUser(RegisterUserDto register);
        Task<LoginUserResult> LoginUser(LoginUserDTO login,bool checkAdminRole);
        bool IsUserExistByEmail(string email);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUserId(long userId);
        Task<User> GetUserByEmailActiveCode(string emailActiveCode);
        void ActivateUser(User user);
        Task EditUserInfo(EditUserDto editUser, long userId);
        Task<bool> IsUserAdmin(long userId);
    }
}
