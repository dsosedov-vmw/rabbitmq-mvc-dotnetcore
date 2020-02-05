using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Web.Models;

namespace Web.Controllers
{
    public class ProbeController : Controller
    {
        const string QueueName = "rabbitmq-mvc-dotnetcore-probe";

        readonly ILogger<ProbeController> _logger;

        public ProbeController(ILogger<ProbeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index([FromServices] ConnectionFactory connectionFactory)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    if (channel.IsOpen)
                    {
                        ViewData["Status"] = "Connection established successfully.";
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send([FromServices] ConnectionFactory connectionFactory, Message message)
        {
            var body = Encoding.UTF8.GetBytes(message.Text);

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    channel.BasicPublish(exchange: "",
                                 routingKey: QueueName,
                                 basicProperties: null,
                                 body: body);
                }
            }

            ViewData["Status"] = "Message sent successfully.";

            return View();
        }

        [HttpGet]
        public IActionResult Receive()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Receive([FromServices] ConnectionFactory connectionFactory)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    // TODO: rewrite this
                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume(queue: QueueName,
                                         autoAck: true,
                                         consumer: consumer);

                    ViewData["Status"] = "No messages found.";

                    var message = string.Empty;

                    while (consumer.Queue.Dequeue(1000, out BasicDeliverEventArgs ea))
                    {
                        message += $"{Encoding.UTF8.GetString(ea.Body)}; ";
                    }

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        ViewData["Status"] = message;
                    }
                }
            }

            return View();
        }
    }
}
