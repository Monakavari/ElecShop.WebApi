using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.Core.DTOs.Products
{
    public class ProductCommentDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Text { get; set; }
        public string CreateDate { get; set; }
    }

    public enum AddProductCommentResult
    {
        Success,
        NotFound,
        Error
    }
}