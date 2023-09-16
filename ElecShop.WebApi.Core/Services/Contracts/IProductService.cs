using ElecShop.Core.DTOs.Products;
using ElecShop.WebApi.Core.DTOs.Products;
using ElecShop.WebApi.DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.WebApi.Core.Services.Contracts
{
    public interface IProductService : IDisposable
    {
        #region product

        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task<FilterProductsDto> FilterProducts(FilterProductsDto filter);
        Task<List<ProductCategory>> GetActiveProductCategories();
        Task<Product> GetProductById(long id);
        Task<List<ProductGallery>> GetActiveProductGallery(long id);
        Task<List<Product>> GetRelatedProducts(long Productid);
        Task AddCommentToProduct(ProductComment comment);
        Task<List<ProductCommentDto>> GetActiveProductComments(long id);
        Task <ProductCommentDto> AddProductComment(AddProductCommentDto comment, long userId);
        Task<bool> IsExistProduct(long id);
        Task<Product> GetProductForUserOrder(long productId);
        Task<EditProductDto> GetProductForEdit(long productId);
        Task EditProduct(EditProductDto product);

    }

    #endregion
}

