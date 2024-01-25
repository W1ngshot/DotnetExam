using DotnetExam.Hubs;
using DotnetExam.Models.Events;
using DotnetExam.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DotnetExam.Services;

public class EventSenderService(IHubContext<RoomHub, IRoom> hubContext) : IEventSenderService
{
    public async Task SendGameStartEvent(GameStartEvent @event)
    {
        await hubContext.Clients.Group(@event.GameId.ToString()).GameStart(@event);
    }

    public async Task SendPlayerMoveEvent(PlayerMoveEvent @event)
    {
        await hubContext.Clients.Group(@event.GameId.ToString()).PlayerMove(@event);
    }

    public async Task SendGameOverEvent(GameOverEvent @event)
    {
        await hubContext.Clients.Group(@event.GameId.ToString()).GameOver(@event);
    }

    public async Task SendGameRestartEvent(GameRestartEvent @event)
    {
        await hubContext.Clients.Group(@event.GameId.ToString()).GameRestart(@event);
    }

    public async Task SendMessageEvent(SendMessageEvent @event)
    {
        await hubContext.Clients.Group(@event.GameId.ToString()).SendMessage(@event);
    }
}