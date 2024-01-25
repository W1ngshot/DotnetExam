using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;

namespace DotnetExam.Features.Game.JoinGame;

public record JoinGameResponse(
    Guid HostId,
    string HostName,
    Guid? OpponentId,
    string? OpponentName,
    GameState State,
    Board Board,
    Mark? Mark); 