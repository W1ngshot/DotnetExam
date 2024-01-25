using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.Connect;

public record ConnectResponse(Guid HostId, string HostName, Guid? OpponentId, string? OpponentName, GameState State);