using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TSMbank.Models;

namespace TSMbank.Hubs
{
    public class SignalHub : Hub
    {

        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();

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



    }
}