using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using OrdersAPI.Core;
using OrdersAPI.Services;

namespace OrdersAPI.Controllers
{
    [Authorize]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        protected ResponseDTO _response;
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
            this._response = new ResponseDTO();
        }

        [HttpGet]
        [Route("GetOrdersList")]
        public async Task<ResponseDTO> GetOrdersList([FromQuery] OrdersPageParams pagingParams)
        {
            try
            {
                _response.Result = HandlePagedResult(await _ordersService.List(pagingParams));

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        protected ActionResult HandlePagedResult(PagedList<OrderDTO> result)
        {
            if (result == null) return NotFound();

            Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);
            return Ok(result);
        }

    }
}
