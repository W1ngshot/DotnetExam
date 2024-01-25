using DotnetExam.Models.Events;

namespace DotnetExam.Hubs;

public interface IRoom
{
    Task GameStart(GameStartEvent @event);
    Task PlayerMove(PlayerMoveEvent @event);
    Task GameOver(GameOverEvent @event);
    Task GameRestart(GameRestartEvent @event);
    Task SendMessage(SendMessageEvent @event);
}