using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSockets_applications.SocketManager;

namespace WebSockets_applications.SocketManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager ConnectionManager { get; set; }

        public SocketHandler(ConnectionManager connectionManager) { ConnectionManager = connectionManager; }

        public virtual async Task OnConnection(WebSocket socket)
        {
            await Task.Run(() => { ConnectionManager.AddSocket(socket); });
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await ConnectionManager.RemoveSocketAsync(ConnectionManager.GetId(socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if(socket.State != WebSocketState.Open) return;
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message),0, message.Length),
                WebSocketMessageType.Text,true, CancellationToken.None);
        }

        public async Task SendedMessage(string id, string message)
        {
            await SendMessage(ConnectionManager.GetSocketById(id), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach(var con in ConnectionManager.GetAllConnections())
                await SendMessage(con.Value, message);
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

    }

   
}
