using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Burgers.Api.Hubs
{
    [Authorize]
    public class BookingsHub : Hub
    {
    }
}
