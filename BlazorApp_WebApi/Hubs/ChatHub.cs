using Microsoft.AspNetCore.SignalR;
using Models;
using Newtonsoft.Json;
using Services;
namespace BlazorApp_WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private UserService _userService;
        public ChatHub(UserService userService)
        {
            _userService = userService;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",user,message);
        }
    }
}
