using DotnetExam.Models;

namespace DotnetExam.Features.Game.SendMessage;

public record SendMessageResponse(Guid GameId, string IdempotenceKey, string Message, UserInfo Sender);