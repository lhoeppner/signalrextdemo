using Microsoft.AspNet.SignalR;

namespace SignalrRestart
{
    internal class Class1
    {
        private readonly string connectionId;
        private readonly IHubContext context;

        public Class1(IHubContext context, string connectionId)
        {
            this.context = context;
            this.connectionId = connectionId;
        }

        public dynamic Client
        {
            get { return context.Clients.Client(connectionId); }
        }

        public void CallClient()
        {
            Client.callMeBack(" Class1");
        }
    }
}