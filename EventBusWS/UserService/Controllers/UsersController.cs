using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using UserService.Data;
using UserService.Entities;
using UserService.Events;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServiceContext _context;
        private readonly IEventBus _eventBus;

        public UsersController(UserServiceContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var userCreatedEvent = new UserCreatedEvent(user.Name, user.Mail, user.OtherData);
            _eventBus.Publish(userCreatedEvent);

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        //private void PublishToMessageQueue(string integrationEvent, string eventData)
        //{
        //    // TOOO: Reuse and close connections and channel, etc, 
        //    var factory = new ConnectionFactory();
        //    var connection = factory.CreateConnection();
        //    var channel = connection.CreateModel();
        //    var body = Encoding.UTF8.GetBytes(eventData);
        //    channel.BasicPublish(exchange: "user",
        //                                     routingKey: integrationEvent,
        //                                     basicProperties: null,
        //                                     body: body);
        //}
    }
}
