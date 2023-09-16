using ElecShop.WebApi.Core.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.Core.Utilities.Extensions.Paging
{
    public static class PagingExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> queriable, BasePaging pager)
        {
            return queriable.Skip(pager.SkipEntity).Take(pager.TakeEntity);
        }
    }
}
