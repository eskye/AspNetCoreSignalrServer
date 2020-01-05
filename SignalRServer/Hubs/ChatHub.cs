using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignalRServer
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("---> Connection Established  " + Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("ReceivedConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            var routeObj = JsonConvert.DeserializeObject<Payload>(message);
            Console.WriteLine("Message Received on: " + Context.ConnectionId);
            Console.WriteLine("Message : " + routeObj.Message);

            if(routeObj.To == string.Empty)
            {
               await Clients.All.SendAsync("ReceiveMessage", message);
            }
            else
            {
                await Clients.Client(routeObj.To).SendAsync("ReceiveMessage", message);
            }
        }
    }


    public class Payload{
        public string To{get;set;}
        public string From {get;set;}
        public string Message{get;set;}
    }
}