using ElecShop.Core.Services.Contracts;
using ElecShop.WebApi.Core.Utilities.Common;
using ElecShop.WebApi.Core.Utilities.Extensions.Identity;
using ElecShop.WebApi.DataLayer.Entities.Account;
using Microsoft.AspNetCore.Mvc;

namespace ElecShop.WebApi.Controllers
{
    public class OrderController : SiteBaseController
    {
        #region Constructor

        public readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion Constructor

        #region Add product to order

        [HttpGet("add-order")]
        public async Task<IActionResult> AddProductToOrder(long productId, int count)
        {
            if (User.Identity.IsAuthenticated)
            {

                var userId = User.GetUserId();

                await _orderService.AddProductToOrder(userId, productId, count);

                return JsonResponseStatus.Success(new
                {
                    message = "Add successfully was done",
                    returndata = _orderService.GetUserBasketDetails(userId)
                });
            }

            return JsonResponseStatus.Error("Add was failed");
        }

        #endregion Add product to order

        #region User backet details

        [HttpGet("use-order-details")]

        public async Task<IActionResult> GetUserBasketDetails()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                await _orderService.GetUserBasketDetails(userId);

                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion User backet details

        #region Remove order detail from backet 

        [HttpGet("Remove-order-detail/{DetailId}")]

        public async Task<IActionResult> RemoveOrderDetailFromBacket(int DetailId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var openOrder = await _orderService.GetUserOpenOrder(User.GetUserId());
                var detail = openOrder.OrderDetail.SingleOrDefault(x => x.Id == DetailId);

                if (detail != null)
                    await _orderService.RmoveOrderDetail(detail);

                return JsonResponseStatus.Success(await _orderService.GetUserBasketDetails(User.GetUserId()));
            }

            return JsonResponseStatus.Error();
        }

        #endregion Remove order detail from backet 

    }
}
