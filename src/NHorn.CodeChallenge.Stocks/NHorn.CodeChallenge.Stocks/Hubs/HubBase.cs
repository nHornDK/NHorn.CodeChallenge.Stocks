using Microsoft.AspNetCore.SignalR;
using NHorn.Utilities;

namespace NHorn.CodeChallenge.Stocks.Hubs
{
    public class HubBase<T> : Hub<T> where T : class
    {
        public IEventBus Eventbus { get; }


        public HubBase(IEventBus eventbus)
        {
            Eventbus = eventbus;
        }
    }
}
