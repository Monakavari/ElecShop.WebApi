using ElecShop.Core.DTOs.Products;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace ElecShop.WebApi.Controllers
{
    public class AdminProductsController : SiteBaseController
    {
        #region Constructor

        private readonly IProductService _productService;
        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion Constructor

        #region product-for-edit

        [HttpGet("product-for-edit/{id}")]
        public async Task<IActionResult> GetProductForEdit(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);

            if (product == null) return JsonResponseStatus.NotFound();

            return JsonResponseStatus.Success(product);
        }
        #endregion product-for-edit

        #region edit-product

        [HttpPost("edit-product/{id}")]
        public async Task<IActionResult> EditProduct(EditProductDto product)
        {
            if (ModelState.IsValid)
            {
                await _productService.EditProduct(product);
                return JsonResponseStatus.Success();

            }
            return JsonResponseStatus.Error();
        }

        #endregion edit-product



    }
}
