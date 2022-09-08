using coffee_kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Hubs
{
    public class SystemEventHub : Hub
    {
        public static string KIOSK_CONNECTION_CHANNEL = "KIOSK_CONNECTION_CHANNEL";

        public async Task JoinRoom(KioskConnectionViewModel kioskConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, kioskConnection.RoomId);
            Console.WriteLine($"{kioskConnection.KioskId} has joined {kioskConnection.RoomId}");
            await Clients.Group(kioskConnection.KioskId)
                .SendAsync("KIOSK_MESSAGE_CONNECTED_CHANNEL",
                    "SYSTEM_BOT", "Connected On Kiosk System Success");
        }
    }
}
