using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Hubs;

// inherits from hub so it can access all the premade codebehind
public class NotificationHub : Hub
{
    // async that does not return a value. it takes 1 parameter of type object
    public async Task SendNotification(object notification)
    {
        //this will trigger the javascript in _notification dropdown partial
        // connection.on("recieve") catches the messages and can work with it via javascript. no pagereload
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }
}
