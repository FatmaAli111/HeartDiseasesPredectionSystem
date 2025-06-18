// DoctorPatientChatController.cs
using DataAcess;
using DataAcess.Repos.IRepos;
using IdentityManagerAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System.Security.Claims;

namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorPatientChatController : ControllerBase
    {
        private readonly IMessage _messagerepo;
        private readonly ApplicationDbContext _context;

        public IHubContext<ChatHub> _hubContext { get; }

        public DoctorPatientChatController(IMessage messagerepo, IHubContext<ChatHub> hubContext,ApplicationDbContext context)
        {
            _messagerepo = messagerepo;
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            var senderIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(senderIdClaim, out Guid senderId))
                return Unauthorized();

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow
            };

            await _messagerepo.AddAsync(message);

            await _hubContext.Clients.User(message.ReceiverId.ToString())
                .SendAsync("ReceiveMessage", message.SenderId, message.Content);

            return Ok(message);
        }

        [HttpGet("conversation")]
        public async Task<List<Message>> GetConversation(Guid user2Id)
        {
            var senderIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(senderIdClaim, out Guid senderId))
                return new List<Message>(); 

            return await _context.messages
                .Where(m =>
                    (m.SenderId == senderId && m.ReceiverId == user2Id) ||
                    (m.SenderId == user2Id && m.ReceiverId == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
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