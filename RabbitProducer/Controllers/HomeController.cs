using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RabbitProducer.Services;
using Models;

namespace RabbitProducer.Controllers
{
    [Authorize]
    [Route("api/rabbitsend")]
    public class HomeController : ControllerBase
    {
        private readonly IRabbitService _rabbitService;
        public HomeController(IRabbitService rabbitService) 
        {  
            _rabbitService = rabbitService;
        }
        [HttpPost]
        public void Index([FromBody]LogMessageDTO message)
        {
            _rabbitService.SendMessage(message);
            return;
        }
    }
}
