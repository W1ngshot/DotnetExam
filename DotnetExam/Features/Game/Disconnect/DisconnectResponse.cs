using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.Disconnect;

public record DisconnectResponse(bool IsOpponentWon, Guid? WinnerId, GameState State, Guid GameId);