// DoctorPatientChatController.cs
using DataAcess.Repos.IRepos;
using IdentityManagerAPI.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.Domain;

namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorPatientChatController : ControllerBase
    {
        private readonly IMessage _messagerepo;
        public IHubContext<ChatHub> _hubContext { get; }

        public DoctorPatientChatController(IMessage messagerepo, IHubContext<ChatHub> hubContext)
        {
            _messagerepo = messagerepo;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            message.SentAt = DateTime.UtcNow;
            await _messagerepo.AddAsync(message);

            await _hubContext.Clients.User(message.ReceiverId.ToString())
                .SendAsync("ReceiveMessage", message.SenderId, message.Content);

            return Ok(message);
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversationAsync(Guid user1Id, Guid user2Id)
        {
            var messages = await _messagerepo.GetConversation(user1Id, user2Id);
            return Ok(messages);
        }

        [HttpGet("conversationByDate")]
        public async Task<IActionResult> GetConversationByDate(Guid user1Id, Guid user2Id, DateTime date)
        {
            var messages = await _messagerepo.GetConversation(user1Id, user2Id);
            var filtered = messages.Where(m => m.SentAt.Date == date.Date);
            return Ok(filtered);
        }
    }
}