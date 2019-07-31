using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace TSMbank.Hubs
{
    public class SignalHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();


        //test code
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


        //good code
        public static void GetRequest(Object request1)
        {
            hubContext.Clients.All.getRequests(request1);
        }
        public static void GetAccRequest(Object request2)
        {
            hubContext.Clients.All.getAccRequests(request2);
        }

        public static void GetTransactions(Object transaction)
        {
            hubContext.Clients.All.showTransactions(transaction);

        }
    }
}