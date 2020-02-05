using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Web.Models;

namespace Web.Controllers
{
    public class ProbeController : Controller
    {
        private readonly ILogger<ProbeController> _logger;

        public ProbeController(ILogger<ProbeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index([FromServices] ConnectionFactory connectionFactory)
        {
            var model = new ProbeViewModel();

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    if (channel.IsOpen)
                    {
                        model.Status = "Success";
                    }

                    //CreateQueue(channel);
                    //var body = Encoding.UTF8.GetBytes("a message");
                    //channel.BasicPublish(exchange: "",
                    //                     routingKey: "a-topic",
                    //                     basicProperties: null,
                    //                     body: body);

                }
            }

            return View(model);
        }
    }
}
