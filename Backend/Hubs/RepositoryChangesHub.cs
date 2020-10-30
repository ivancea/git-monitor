using System.Collections.Generic;
using System.Threading.Tasks;
using GitMonitor.Objects.Changes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GitMonitor.Hubs
{
    /// <summary>
    /// SignalR hub to broadcast repository changes.
    /// </summary>
    [Authorize]
    public class RepositoryChangesHub : Hub
    {
    }
}