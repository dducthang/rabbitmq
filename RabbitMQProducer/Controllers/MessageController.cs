using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQProducer.Producer;

namespace RabbitMQProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        public MessageController(
                IMessageProducer messageProducer
            )
        {
            _messageProducer = messageProducer;
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] Models.Message message)
        {
            try
            {
                _messageProducer.SendMessage(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
