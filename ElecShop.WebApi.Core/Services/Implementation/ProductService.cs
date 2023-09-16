using ElecShop.Core.DTOs.Products;
using ElecShop.Core.Utilities.Common;
using ElecShop.Core.Utilities.FileUploder;
using ElecShop.WebApi.Core.DTOs.Paging;
using ElecShop.WebApi.Core.DTOs.Products;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Utilities.Extensions.Paging;
using ElecShop.WebApi.DataLayer.Entities.Product;
using ElecShop.WebApi.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace ElecShop.WebApi.Core.Services.Implementation
{
    public class ProductService : IProductService
    {

        #region constructor

        private IGenericRepository<Product> productRepository;
        private IGenericRepository<ProductCategory> productCategoryRepository;
        private IGenericRepository<ProductGallery> productGalleryRepository;
        private IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository;
        private IGenericRepository<ProductVisit> productVisitRepository;
        private IGenericRepository<ProductComment> ProductCommentRepository;

        public ProductService(IGenericRepository<Product> productRepository,
                              IGenericRepository<ProductCategory> productCategoryRepository,
                              IGenericRepository<ProductGallery> productGalleryRepository,
                              IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository,
                              IGenericRepository<ProductVisit> productVisitRepository,
                              IGenericRepository<ProductComment> productCommentRepository)
        {
            this.productRepository = productRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.productGalleryRepository = productGalleryRepository;
            this.productSelectedCategoryRepository = productSelectedCategoryRepository;
            this.productVisitRepository = productVisitRepository;
            this.ProductCommentRepository = productCommentRepository;
        }

        #endregion

        #region product

        public async Task<Product> GetProductById(long id)
        {
            var product = await productRepository
                               .GetEntitiesQuery()
                               .SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            return product;
        }

        public async Task AddProduct(Product product)
        {
            await productRepository.AddEntity(product);
            await productRepository.SaveChanges();
        }

        public async Task UpdateProduct(Product product)
        {
            productRepository.UpdateEntity(product);
            await productRepository.SaveChanges();
        }

        public async Task<List<Product>> GetRelatedProducts(long Productid)
        {
            var product = await productRepository.GetEntityById(Productid);
            if (product == null) return null;

            var ProductCategoriesList = await productSelectedCategoryRepository
                        .GetEntitiesQuery()
                        .Where(x => x.ProductId == Productid)
                        .Select(s => s.ProductCategoryId)
                        .ToListAsync();

            var relatedProduct = await productSelectedCategoryRepository
                        .GetEntitiesQuery()
                        .Where(s => ProductCategoriesList.Contains(s.ProductCategoryId))
                        .Select(t => t.Product)
                        .Where(s => s.Id != Productid)
                        .OrderByDescending(x => x.CreateDate)
                        .Take(4)
                        .ToListAsync();

            return relatedProduct;

        }

        public async Task<FilterProductsDto> FilterProducts(FilterProductsDto filter)
        {
            var productQuery = productRepository.GetEntitiesQuery().AsQueryable();

            switch (filter.OrderBy)
            {
                case FilterProductsDto.ProductOrderBy.PriceAsc:
                    productQuery = productQuery.OrderBy(x => x.Price);
                    break;

                case FilterProductsDto.ProductOrderBy.PriceDsc:
                    productQuery = productQuery.OrderByDescending(x => x.Price);
                    break;
            }
            if (filter.CategoryIds != null && filter.CategoryIds.Any())
                productQuery = productQuery.SelectMany(x => x.ProductSelectedCategories.
                Where(x => filter.CategoryIds.Contains(x.ProductCategoryId)).Select(x => x.Product));

            if (!string.IsNullOrEmpty(filter.Title))
                productQuery = productQuery.Where(x => x.ProductName.Contains(filter.Title));

            if (filter.StartPrice != null)
                productQuery = productQuery.Where(x => x.Price >= filter.StartPrice);

            if (filter.EndPrice != null)
                productQuery = productQuery.Where(x => x.Price <= filter.EndPrice);

            var pageCount = (int)Math.Ceiling(productQuery.Count() / (double)filter.TakeEntity);

            var pager = Pager.Build(pageCount, filter.PageId, filter.TakeEntity);

            var product = await productQuery.Paging(pager).ToListAsync();

            return filter.SetProduct(product).SetPaging(pager);
        }

        public async Task<bool> IsExistProduct(long id)
        {
            return await productRepository.GetEntitiesQuery().AnyAsync(x => x.Id == id);
        }

        public async Task<Product> GetProductForUserOrder(long productId)
        {
            return await productRepository.GetEntitiesQuery().SingleOrDefaultAsync(x => x.Id == productId && !x.IsDelete);
        }

        public async Task<EditProductDto> GetProductForEdit(long productId)
        {
            var product = await productRepository.GetEntitiesQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == productId);

            if (product == null) return null;

            return new EditProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                CurrentImage = product.ImageName,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                IsExists = product.IsExists,
                IsSpecial = product.IsSpecial
            };
        }

        public async Task EditProduct(EditProductDto product)
        {
            var mainProduct = await GetProductById(product.Id);

            if (mainProduct != null)
            {
                mainProduct.ProductName = product.ProductName;
                mainProduct.Price = product.Price;
                mainProduct.Description = product.Description;
                mainProduct.ShortDescription = product.ShortDescription;
                mainProduct.IsExists = product.IsExists;
                mainProduct.IsSpecial = product.IsSpecial;

                if (!string.IsNullOrEmpty(product.Base64Image))
                {
                    var imageFile = ImageUploaderExtension.Base64ToImage(product.Base64Image);
                    var imageName = Guid.NewGuid().ToString("N") + ".jpeg";
                    imageFile.AddImageToServer(imageName, PathTools.ProductServerImagePath, mainProduct.ImageName);
                    mainProduct.ImageName = imageName;
                }

                productRepository.UpdateEntity(mainProduct);
                await productRepository.SaveChanges();

            };

        }

        #endregion

        #region ProductCategory
        public async Task<List<ProductCategory>> GetActiveProductCategories()
        {
            return await productCategoryRepository.GetEntitiesQuery()
                .Where(x => x.IsDelete == false)
                .ToListAsync();
        }

        #endregion ProductCategory

        #region Product gallery
        public async Task<List<ProductGallery>> GetActiveProductGallery(long productId)
        {
            return await productGalleryRepository.GetEntitiesQuery()
                .Where(x => x.ProductId == productId && x.IsDelete == false)
                .Select(x => new ProductGallery
                {
                    ProductId = x.ProductId,
                    ImageName = x.ImageName,
                    Id = x.Id,
                    CreateDate = x.CreateDate
                })
                .ToListAsync();
        }
        #endregion Product gallery

        #region Product Comment
        public async Task AddCommentToProduct(ProductComment comment)
        {
            await ProductCommentRepository.AddEntity(comment);
            await ProductCommentRepository.SaveChanges();
        }

        public async Task<List<ProductCommentDto>> GetActiveProductComments(long id)
        {
            return await ProductCommentRepository.GetEntitiesQuery().Include(s => s.User).Where(x => x.Id == id && x.IsDelete == false)
                   .OrderByDescending(x => x.CreateDate)
                  .Select(s => new ProductCommentDto
                  {
                      Id = s.Id,
                      UserId = s.UserId,
                      Text = s.Text,
                      FullName = s.User.FirstName + "" + s.User.LastName,
                      CreateDate = s.CreateDate.ToString("yyyy/MM/DD HH:MM")

                  }).ToListAsync();
        }

        public async Task<ProductCommentDto> AddProductComment(AddProductCommentDto comment, long userId)
        {
            var product = await GetProductById(comment.ProductId);

            //if (product == null)
            //    return AddProductCommentResult.NotFound;

            var result = new ProductComment
            {
                ProductId = comment.ProductId,
                UserId = userId,
                Text = comment.Text
            };

            await ProductCommentRepository.AddEntity(result);
            await ProductCommentRepository.SaveChanges();

            return new ProductCommentDto
            {
                Id = result.Id,
                UserId = userId,
                Text = result.Text,
                CreateDate = result.CreateDate.ToString("yyyy,MM,dd HH:MM"),
                FullName = ""
            };

        }

        #endregion Product Comment

        #region dispose
        public void Dispose()
        {
            productRepository?.Dispose();
            productCategoryRepository?.Dispose();
            productGalleryRepository?.Dispose();
            productSelectedCategoryRepository?.Dispose();
            productVisitRepository?.Dispose();
            ProductCommentRepository?.Dispose();
        }

        #endregion
    }
}
