using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.Core.Services.Contracts
{
    public interface IAccessService : IDisposable
    {
        #region User role
        Task<bool> CheckUserRole(long userId, string role);

        #endregion User role
    }
}
