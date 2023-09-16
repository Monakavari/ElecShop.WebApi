using ElecShop.WebApi.DataLayer.Entities.Common;
using ElecShop.WebApi.DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.DataLayer.Entities.Order
{
    public class OrderDetail : BaseEntity
    {
        #region Properties
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }

        #endregion Properties

        #region Relations
        public Order Order { get; set; }
        public Product Product { get; set; }

        #endregion Relations

    }
}
