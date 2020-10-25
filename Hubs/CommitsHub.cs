using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GitMonitor
{
    /// <summary>
    /// SignalR hub to broadcast new commits.
    /// </summary>
    public class CommitsHub : Hub
    {
        /// <summary>
        /// Broadcasts new commits message to all clients.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task BroadcastNewCommits()
        {
            await Clients.All.SendAsync("newCommits");
        }
    }
}