using ElecShop.WebApi.Core.DTOs.Products;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Services.Implementation;
using ElecShop.WebApi.Core.Utilities.Common;
using ElecShop.WebApi.Core.Utilities.Extensions.Identity;
using ElecShop.WebApi.DataLayer.Entities.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Drawing;

namespace ElecShop.WebApi.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Constructor 

        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion Constructor 

        #region Product

        [HttpGet("filter-products")]
        public async Task<IActionResult> GetProducts([FromQuery] FilterProductsDto filter)
        {
            var products = await _productService.FilterProducts(filter);
            return JsonResponseStatus.Success(products);
        }

        #endregion Product

        #region ProductCategory

        [HttpGet("product-Active-categories")]
        public async Task<IActionResult> GetActiveProductCategories()
        {
            return JsonResponseStatus.Success(await _productService.GetActiveProductCategories());
        }

        #endregion ProductCategory

        #region single product

        [HttpGet("single-product/{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _productService.GetProductById(id);
            var productGallery = await _productService.GetActiveProductGallery(id);

            if (product != null)
                return JsonResponseStatus.Success(new { product = product, productGallery = productGallery });

            return JsonResponseStatus.NotFound();
        }
        #endregion single product

        #region Related Product

        [HttpGet("related-product/{id}")]
        public async Task<IActionResult> GetRelatedProducts(long Productid)
        {
            var relatedProduct = await _productService.GetRelatedProducts(Productid);
            return JsonResponseStatus.Success(relatedProduct);
        }

        #endregion Related Product

        #region Product Comment

        [HttpGet("product-comment/{id}")]
        public async Task<IActionResult> GetActiveProductComments(long id)
        {
            var commentProduct = await _productService.GetActiveProductComments(id);
            return JsonResponseStatus.Success(commentProduct);
        }

        [HttpPost("add-product-comment")]
        public async Task<IActionResult> AddProductComments(AddProductCommentDto comment)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error(new { Message = "please login" });

            if (!await _productService.IsExistProduct(comment.ProductId))
                return JsonResponseStatus.NotFound();


            var userId = User.GetUserId();
            await _productService.AddProductComment(comment, userId);

            return JsonResponseStatus.Success();
        }
        #endregion Product Comment
    }


}

