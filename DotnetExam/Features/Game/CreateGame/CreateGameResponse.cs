using DotnetExam.Models;

namespace DotnetExam.Features.Game.CreateGame;

public record CreateGameResponse(Guid GameId, PlayerInfo Host);