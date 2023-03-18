
using Microsoft.AspNetCore.SignalR;

namespace ChatAppGroup.Hubs
{
    public class ChatHub : Hub
    {
       // chatHub with group, user , message and recivce
       public void SendMessageToGroup(string groupId, string user, string message)
        {
            Clients.Group(groupId).SendAsync("ReceiveMessage", user, message);
        }

        public void JoinGroup(string groupId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public void RemoveFromGroup(string groupId)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
