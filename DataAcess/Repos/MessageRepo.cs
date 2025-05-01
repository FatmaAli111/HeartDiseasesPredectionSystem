using DataAcess.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos
{
    public class MessageRepo : Repository<Message>,IMessage
    {
        private readonly ApplicationDbContext db;

        public MessageRepo(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<List<Message>> GetConversation(Guid user1Id, Guid user2Id)
        {
            List<Message> messages = new List<Message>();
            try
            {
                 messages= await db.messages
                  .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                      (m.SenderId == user2Id && m.ReceiverId == user1Id))
                  .OrderBy(m => m.SentAt)
                  .ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message); // أو استخدم اللوجينج

            }
            return messages;
        }
    }
}
