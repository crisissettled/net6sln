using Microsoft.AspNetCore.SignalR;

namespace web.api.Utility.signalR
{
    public class MyHub : Hub
    {
        public Task SendPublicMessage(string message,string user)
        {
            string connId = Context.ConnectionId;
            string msgToSend = $"{message} - {DateTime.Now.ToShortTimeString()}";

            return Clients.All.SendAsync("PublicMessageReceived", new { message = msgToSend, user });
        }
    }
}
