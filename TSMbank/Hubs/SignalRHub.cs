using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TSMbank.Hubs
{
    public class SignalRHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();

        public override Task OnConnected()
        {
            string name = Context.User.Identity.GetUserId();


            return base.OnConnected();
        }

        public static void Static_Send()
        {
            hubContext.Clients.All.test("New message");
        }

        public void Hello(string message)
        {
            Console.WriteLine("Malakokavlis");
            Clients.All.send("connected", message);
        }

        public static void GetRequest(Object request)
        {
            hubContext.Clients.All.getRequest(request);
        }

        public static void GetTransactions(Object transaction)
        {
            hubContext.Clients.All.showTransactions(transaction);

        }
    }
}