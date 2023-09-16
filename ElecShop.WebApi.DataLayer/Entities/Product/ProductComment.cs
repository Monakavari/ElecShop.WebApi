using ElecShop.WebApi.DataLayer.Entities.Account;
using ElecShop.WebApi.DataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElecShop.WebApi.DataLayer.Entities.Product
{
    public class ProductComment :BaseEntity
    {
        public long ProductId { get; set; }
        public long UserId { get; set; }

        [Display(Name = "نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Text { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}
