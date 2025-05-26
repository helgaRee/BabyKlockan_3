using Microsoft.AspNetCore.SignalR;

namespace BabyKlockan_3.Hubs;

public class ContractionHub : Hub
{
    //lägg till metoder som klienten kan anropa här
    public async Task SendContractionUpdate()
    {
        await Clients.All.SendAsync("RecieveContractionUpdate");
    }
}
