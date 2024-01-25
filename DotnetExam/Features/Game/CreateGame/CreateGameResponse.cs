using DotnetExam.Models.Enums;

namespace DotnetExam.Features.Game.CreateGame;

public record CreateGameResponse(Guid GameId, Mark HostMark);