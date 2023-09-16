using ElecShop.Core.DTOs.Orders;
using ElecShop.DataLayer.Entities.Order;
using ElecShop.WebApi.DataLayer.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.Core.Services.Contracts
{
    public interface IOrderService : IDisposable
    {
        #region Order
        Task<Order> CreateUserOrder(long userId);
        Task<Order> GetUserOpenOrder(long userId);

        #endregion Order

        #region OrderDetail
        Task AddProductToOrder(long userId, long productId, int count);
        Task<List<OrderDetail>> GetOrderDetails(long orderId);
        Task<List<OrderBasketDetails>> GetUserBasketDetails(long userId);
        Task RmoveOrderDetail(OrderDetail orderDetail);

        #endregion OrderDetail



    }
}
