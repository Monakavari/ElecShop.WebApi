using ElecShop.WebApi.Core.DTOs.Paging;
using ElecShop.WebApi.DataLayer.Entities.Product;

namespace ElecShop.WebApi.Core.DTOs.Products
{
    public class FilterProductsDto : BasePaging
    {
        public string Title { get; set; }
        public int StartPrice { get; set; }
        public int EndPrice { get; set; }
        public List<Product> products { get; set; }
        public List<long> CategoryIds { get; set; }
        public ProductOrderBy? OrderBy { get; set; }
        public FilterProductsDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.ActivePage = paging.ActivePage;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;

            return this;
        }
        public FilterProductsDto SetProduct(List<Product> product)
        {
            this.products = product;
            return this;
        }

        public enum ProductOrderBy
        {
        PriceAsc,
        PriceDsc,
        }
    }
}
