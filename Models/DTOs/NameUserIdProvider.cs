//using Microsoft.AspNet.SignalR.Hubs;
//using Microsoft.AspNet.SignalR;using Microsoft.AspNet.SignalR;
//using Microsoft.AspNetCore.SignalR;
//using System.Security.Claims;

//public class NameUserIdProvider : IUserIdProvider
//{
//    public string? GetUserId(HubConnectionContext connection)
//    {
//        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    }

//    public string GetUserId(IRequest request)
//    {
//        throw new NotImplementedException();
//    }
//}
