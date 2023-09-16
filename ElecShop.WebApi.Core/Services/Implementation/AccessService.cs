using ElecShop.Core.Services.Contracts;
using ElecShop.WebApi.DataLayer.Entities.Access;
using ElecShop.WebApi.DataLayer.Entities.Account;
using ElecShop.WebApi.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.Core.Services.Implementation
{
    public class AccessService : IAccessService
    {
        #region Constructor

        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<UserRole> _userRoleRepository;
        public AccessService(IGenericRepository<Role> roleRepository,
                             IGenericRepository<User> userRepository,
                             IGenericRepository<UserRole> userRoleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        #endregion Constructor


        #region User role
        public async Task<bool> CheckUserRole(long userId, string role)
        {
            return await _userRoleRepository.GetEntitiesQuery().AsQueryable().AnyAsync(x => x.UserId == userId && x.Role.Name == role);
        }

        #endregion User role

        #region Dispose
        public void Dispose()
        {
            _roleRepository?.Dispose();
            _userRepository.Dispose();
            _userRoleRepository.Dispose();
        }

        #endregion Dispose
    }

}
