﻿using ElecShop.WebApi.DataLayer.Entities.Account;
using ElecShop.WebApi.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.DataLayer.Entities.Access
{
    public class UserRole : BaseEntity
    {
        #region properties

        public long UserId { get; set; }

        public long RoleId { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }

        public Role Role { get; set; }

        #endregion
    }
}
