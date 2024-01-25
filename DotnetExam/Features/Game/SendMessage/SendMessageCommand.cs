using DotnetExam.Infrastructure.Mediator.Command;

namespace DotnetExam.Features.Game.SendMessage;

public record SendMessageCommand(Guid UserId, Guid GameId, string Message, string IdempotenceKey)
    : ICommand<SendMessageResponse>;