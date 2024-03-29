﻿using MainAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using OrdersAPI.Core;
using OrdersAPI.Repository;
using OrdersAPI.Models;

namespace OrdersAPI.Controllers
{
    [Authorize]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepository;
        protected ResponseDTO _response;
        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
            this._response = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> Get([FromBody] OrdersPageParams pagingParams)
        {
            try
            {
                _response.Result = JsonConvert.SerializeObject(await _ordersRepository.List(pagingParams));
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDTO> Get(int id)
        {
            try
            {
                _response.Result = await _ordersRepository.GetOrderById(id);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ResponseDTO> Post([FromBody] OrderDTO orderDTO)
        {
            try
            {
                _response.Result = await _ordersRepository.CreateUpdateOrder(orderDTO);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<ResponseDTO> Delete(int id)
        {
            try
            {
                _response.IsSuccess = await _ordersRepository.DeleteOrder(id);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { e.ToString() };
            }
            return _response;
        }
    }
}
