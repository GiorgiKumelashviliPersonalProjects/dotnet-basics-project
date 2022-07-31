using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // return base.OnConnectedAsync();
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

            await Clients.Group(groupName).SendAsync("ReceivedMessageThread", "sample message");
        }

        private static string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) > 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}