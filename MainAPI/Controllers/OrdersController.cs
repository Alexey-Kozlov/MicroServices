using MainAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using MainAPI.Core;
using System;

namespace MainAPI.Controllers
{
    [Authorize]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrders _orders;
        public OrdersController(IOrders orders)
        {
            _orders= orders;
        }

        [HttpGet("GetOrdersList")]
        public async Task<ResponseDTO> GetOrdersList([FromQuery] OrdersPageParams pagingParams)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _orders.GetOrdersList<ResponseDTO>(accessToken!, pagingParams);
            var respResult = new ResponseDTO();
            if (response != null && response.IsSuccess)
            {
                var pagedList = JsonConvert.DeserializeObject<PagedList<OrderDTO>>(response.Result.ToString()!);
                //добавляем информацию о педжинации в хедер.Можно и без хедера - передавать инфу через модель, это как вариант
                HandlePagedResult(pagedList!);
                respResult.Result = pagedList!.OrderDTO;
                return respResult;
            }
            return respResult;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var response = await _orders.GetOrderById<ResponseDTO>(id, accessToken!);
            if (response != null && response.IsSuccess)
            {
                var rez = JsonConvert.DeserializeObject<OrderDTO>(Convert.ToString(response.Result)!);
                return Ok(rez);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody] OrderDTO orderDTO)
        {
            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                _response.Result = await _categoryRepository.CreateUpdateCategory(categoryDTO);

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
