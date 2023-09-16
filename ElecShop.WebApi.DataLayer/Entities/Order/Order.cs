using ElecShop.WebApi.DataLayer.Entities.Account;
using ElecShop.WebApi.DataLayer.Entities.Common;
using ElecShop.WebApi.DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.DataLayer.Entities.Order
{
    public class Order : BaseEntity
    {
        #region Properties
        public long UserId { get; set; }
        public bool IsPay { get; set; }
        public DateTime? PaymentDate { get; set; }

        #endregion Properties

        #region Relations
        public User User { get; set; }
        public ICollection <OrderDetail> OrderDetail { get; set; }

        #endregion Relations


    }
}
