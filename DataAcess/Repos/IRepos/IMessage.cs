using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Repos.IRepos
{
   public interface IMessage:IRepository<Message>
    {
        Task<List<Message>> GetConversation(Guid user1id,Guid user2id);
    }
}
