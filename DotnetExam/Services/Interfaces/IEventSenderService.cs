using DotnetExam.Models.Events;

namespace DotnetExam.Services.Interfaces;

public interface IEventSenderService
{
    public Task SendGameStartEvent(GameStartEvent @event);
    public Task SendPlayerMoveEvent(PlayerMoveEvent @event);
    public Task SendGameOverEvent(GameOverEvent @event);
    public Task SendGameRestartEvent(GameRestartEvent @event);
    public Task SendMessageEvent(SendMessageEvent @event);
}