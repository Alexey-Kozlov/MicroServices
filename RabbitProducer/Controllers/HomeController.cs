using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RabbitProducer.Services;
using Models;
using Microsoft.Extensions.Logging;

namespace RabbitProducer.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbitService _rabbitService;
        public HomeController(IRabbitService rabbitService, ILogger<HomeController> logger) 
        {  
            _rabbitService = rabbitService;
            _logger = logger;
        }
        [HttpPost("rabbitsend")]
        public void Index([FromBody]LogMessageDTO message)
        {
            if(message == null)
            {
                _logger.LogInformation("message == null!");
                return;
            }
            _rabbitService.SendMessage(message);
            return;
        }
    }
}
