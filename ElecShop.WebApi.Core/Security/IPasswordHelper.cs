using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.Core.Security
{
    public interface IPasswordHelper
    {
        string EncodePasswordMd5(string password);
    }
}
