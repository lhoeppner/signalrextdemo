using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using SignalrRestart;

namespace SignalR.Hosting.Self.Samples
{
    internal class Program
    {
        private static IDisposable signalr;
        private static Thread signalrThread;
        public static Class1 SomeClass;

        private static void Main(string[] args)
        {
            StartThread();
        }

        internal static void ReStartApp()
        {
            signalr.Dispose();
            StartThread();
        }

        internal static void StartThread()
        {
            signalrThread = new Thread(startSignalr);
            signalrThread.Start();
        }

        public static void startSignalr()
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (signalr = WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
                Console.WriteLine("Server stopped");
            }
        }
    }

    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration {EnableJSONP = true, EnableDetailedErrors = true, Resolver = new DefaultDependencyResolver()};

            app.MapSignalR(config);
        }
    }

    public class MyHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            var someClass = new Class1(context, Context.ConnectionId);
            Program.SomeClass = someClass;

            return base.OnConnected();
        }

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        /// <summary>
        /// Call back directly - works
        /// </summary>
        public void CallMeBack2()
        {
            Clients.Caller.callMeBack("hub");
        }

        /// <summary>
        /// Call back through IHubContext fails
        /// </summary>
        public void CallMeBack()
        {
            Program.SomeClass.CallClient();
        }
    }
}