using System.ComponentModel.DataAnnotations;

namespace ElecShop.WebApi.Core.DTOs.Products
{
    public class AddProductCommentDto
    {
        public long ProductId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Text { get; set; }
    }
}
