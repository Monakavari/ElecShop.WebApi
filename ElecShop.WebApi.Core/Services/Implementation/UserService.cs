using ElecShop.Core.DTOs.Account;
using ElecShop.WebApi.Core.DTOs.Account;
using ElecShop.WebApi.Core.Security;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.DataLayer.Entities.Access;
using ElecShop.WebApi.DataLayer.Entities.Account;
using ElecShop.WebApi.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace ElecShop.WebApi.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        #region constructor

        private IGenericRepository<User> userRepository;
        private IGenericRepository<UserRole> userRoleRepository;
        private IPasswordHelper passwordHelper;
        private IMailSender mailSender;
        //private IViewRenderService renderView;

        public UserService(IGenericRepository<User> userRepository,
                           IGenericRepository<UserRole> userRoleRepository,
                           IPasswordHelper passwordHelper,
                           IMailSender mailSender
                           /*IViewRenderService renderView*/)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.passwordHelper = passwordHelper;
            this.mailSender = mailSender;
            //this.renderView = renderView;
        }


        #endregion

        #region User Section

        //***********************************GetAllUsers*****************************************
        public async Task<List<User>> GetAllUsers()
        {
            return await userRepository.GetEntitiesQuery().ToListAsync();
        }

        //***********************************RegisterUser****************************************
        public async Task<RegisterUserResult> RegisterUser(RegisterUserDto register)
        {
            if (IsUserExistByEmail(register.Email))
                return RegisterUserResult.EmailExists;

            var user = new User
            {

                Email = register.Email.SanitizeText(),
                FirstName = register.FirstName.SanitizeText(),
                Address = register.Address.SanitizeText(),
                LastName = register.LastName.SanitizeText(),
                Password = passwordHelper.EncodePasswordMd5(register.Password),
                EmailActiveCode = Guid.NewGuid().ToString(),

            };

            await userRepository.AddEntity(user);
            await userRepository.SaveChanges();
            var body = string.Empty;//await renderView.RenderToStringAsync("Email/ActivateAccount", user);

            mailSender.Send("mona.kavari@gmail.com", "test", body);

            return RegisterUserResult.Success;
        }

        //***********************************IsUserExistByEmail**********************************
        public bool IsUserExistByEmail(string email)
        {
            return userRepository.GetEntitiesQuery().Any(x => x.Email == email.ToLower().Trim());
        }

        //***********************************LoginUser*******************************************
        public async Task<LoginUserResult> LoginUser(LoginUserDTO login, bool checkAdminRole = false)
        {
            var password = passwordHelper.EncodePasswordMd5(login.Password);
            var user = userRepository.GetEntitiesQuery().SingleOrDefault(x => x.Email == login.Email.ToLower().Trim() &&
                                                                         x.Password == password);
            if (user == null)
                return LoginUserResult.IncorrectData;

            if (!user.IsActivated)
                return LoginUserResult.NotActivated;

            if (checkAdminRole)
            {
                if (!await IsUserAdmin(user.Id))
                    return LoginUserResult.NotAdmin;
            }

            return LoginUserResult.Success;

        }

        //***********************************GetUserByEmail**************************************
        public async Task<User> GetUserByEmail(string email)
        {
            return await userRepository.GetEntitiesQuery().SingleOrDefaultAsync(x => x.Email == email.ToLower().Trim());
        }

        //***********************************GetUserByUserId*************************************
        public async Task<User> GetUserByUserId(long userId)
        {
            return await userRepository.GetEntityById(userId);
        }

        //***********************************GetUserByEmailActiveCode****************************
        public async Task<User> GetUserByEmailActiveCode(string emailActiveCode)
        {
            return await userRepository.GetEntitiesQuery().SingleOrDefaultAsync(x => x.EmailActiveCode == emailActiveCode);
        }

        //***********************************ActivateUser****************************************
        public void ActivateUser(User user)
        {
            user.IsActivated = true;
            user.EmailActiveCode = Guid.NewGuid().ToString();
            userRepository.UpdateEntity(user);
            userRepository.SaveChanges();
        }

        //***********************************EditUserInfo****************************************
        public async Task EditUserInfo(EditUserDto editUser, long userId)
        {
            var mainUser = await GetUserByUserId(userId);

            if (mainUser != null)
            {
                mainUser.FirstName = editUser.FirstName;
                mainUser.LastName = editUser.LastName;
                mainUser.Address = editUser.Address;
            };

            userRepository.UpdateEntity(mainUser);
            await userRepository.SaveChanges();
        }

        //***********************************IsUserAdmin****************************************
        public async Task<bool> IsUserAdmin(long userId)
        {
            return await userRoleRepository.GetEntitiesQuery()
                                           .Include(r => r.Role)
                                           .AsQueryable().AnyAsync(x => x.UserId == userId && x.Role.Name == "Admin");
        }
        #endregion

        #region dispose

        public void Dispose()
        {
            userRepository?.Dispose();
        }

        #endregion
    }
}
