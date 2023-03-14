using Covid19ChartSignalR.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace Covid19ChartSignalR.API.Hubs;

public class CovidHub : Hub
{
    private readonly CovidService _covidService;
    public async Task GetCovidList()
    {
        await Clients.All.SendAsync("ReceiveCovidList", _covidService.GetCovidList());
    }
}