using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.Core.Utilities.Extensions.Identity
{
    public static class IdentityUserExtension
    {
        public static long GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var result = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                return Convert.ToInt64(result);
            }
            return default(long);
        }
    }
}
