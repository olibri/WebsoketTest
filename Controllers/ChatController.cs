using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

public class ChatController: Controller{
    public async Task HandleWebSocket(HttpContext httpContext){
        if(httpContext.WebSockets.IsWebSocketRequest ){
            using var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketMessanger(webSocket);            
        }
        else{
            HttpContext.Response.StatusCode = 400;
        }
    }
    private async Task HandleWebSocketMessanger(WebSocket webSocket){
        byte[] buffer = new byte[1024];
        while(webSocket.State == WebSocketState.Open){
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);  
            if(result.MessageType == WebSocketMessageType.Text){
                string messageText = System.Text.Encoding.UTF8.GetString(buffer, 0 , result.Count);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer,0, result.Count), result.MessageType,
                result.EndOfMessage, CancellationToken.None);
            }
        }
    }   
}