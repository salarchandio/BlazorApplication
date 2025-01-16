using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp_Frontend.Services
{
    public class SignalRService : IAsyncDisposable
    {
        private readonly NavigationManager _navigationManager;
        private HubConnection _hubConnection;

        public event Action<string, string>? OnMessageReceived;
        public event Action<string>? OnUserReceived;

        public SignalRService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public async Task InitializeAsync(string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(_navigationManager.ToAbsoluteUri(hubUrl).ToString()))
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                OnMessageReceived?.Invoke(user, message);
            });

            await _hubConnection.StartAsync();
        }

        public async Task InitializeAsyncForUsers(string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(_navigationManager.ToAbsoluteUri(hubUrl).ToString()))
                .Build();

            _hubConnection.On<string>("ReceiveAllUsers", (Result) =>
            {
                OnUserReceived?.Invoke(Result);
            });

            await _hubConnection.StartAsync();
        }

        public async Task SendMessageAsync(string user, string message)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("SendMessage", user, message);
            }
        }

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }

}
