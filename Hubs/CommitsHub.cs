using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GitMonitor.Hubs
{
    /// <summary>
    /// SignalR hub to broadcast new commits.
    /// </summary>
    public class CommitsHub : Hub
    {
        /// <summary>
        /// Broadcasts new commits message to all clients.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task BroadcastNewCommits()
        {
            await Clients.All.SendAsync("newCommits");
        }
    }
}