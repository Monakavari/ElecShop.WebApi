using ElecShop.Core.DTOs.Orders;
using ElecShop.Core.Services.Contracts;
using ElecShop.Core.Utilities.Common;
using ElecShop.DataLayer.Entities.Order;
using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.DataLayer.Entities.Account;
using ElecShop.WebApi.DataLayer.Entities.Product;
using ElecShop.WebApi.DataLayer.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.Core.Services.Implementation
{
    public class OrderService : IOrderService
    {
        #region Constructor

        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public OrderService(IGenericRepository<Order> orderRepository,
                            IGenericRepository<OrderDetail> orderDetailRepository,
                            IUserService userService,
                            IProductService productService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _userService = userService;
            _productService = productService;
        }

        #endregion Constructor

        #region Order
        public async Task<Order> CreateUserOrder(long userId)
        {
            var order = new Order
            { UserId = userId };

            await _orderRepository.AddEntity(order);
            await _orderRepository.SaveChanges();

            return order;
        }
        public async Task<Order> GetUserOpenOrder(long userId)
        {
            var order = await _orderRepository.GetEntitiesQuery()
                .Include(x => x.OrderDetail)
                .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == userId && !x.IsPay);

            if (order == null)
                await CreateUserOrder(userId);

            return order;
        }

        #endregion Order

        #region OrderDetail
        public async Task AddProductToOrder(long userId, long productId, int count)
        {
            var user = await _userService.GetUserByUserId(userId);
            var product = await _productService.GetProductById(productId);

            if (user != null && product != null)
            {
                var order = await GetUserOpenOrder(userId);

                if (count < 1) count = 1;

                var orderDetails = await GetOrderDetails(order.Id);
                var existDetails = orderDetails.SingleOrDefault(x => x.ProductId == productId && !x.IsDelete);

                if (existDetails != null)
                {
                    existDetails.Count += count;
                    _orderDetailRepository.UpdateEntity(existDetails);
                }
                else
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Count = count,
                        Price = product.Price
                    };

                    await _orderDetailRepository.AddEntity(orderDetail);

                }
                await _orderDetailRepository.SaveChanges();

            }
        }
        public async Task<List<OrderDetail>> GetOrderDetails(long orderId)
        {

            return await _orderDetailRepository.GetEntitiesQuery()
                                               .Where(x => x.OrderId == orderId)
                                               .ToListAsync();
        }
        public async Task<List<OrderBasketDetails>> GetUserBasketDetails(long userId)
        {
            var openOrder = await GetUserOpenOrder(userId);

            if (openOrder == null) return null;

            return openOrder.OrderDetail.Where(x => !x.IsDelete).Select(f => new OrderBasketDetails
            {
                Title = f.Product.ProductName,
                Count = f.Count,
                Price = f.Price,
                ImageName = PathTools.Domain + PathTools.ProductImagePath + f.Product.ImageName

            }).ToList();

        }
        public async Task RmoveOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailRepository.RemoveEntity(orderDetail);
            await _orderDetailRepository.SaveChanges();
        }

        #endregion OrderDetail

        #region Disposable
        public void Dispose()
        {
            _orderRepository.Dispose();
            _orderDetailRepository.Dispose();
        }

        #endregion Disposable
    }


}
