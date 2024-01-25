using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;

namespace DotnetExam.Features.Game.RestartGame;

public record RestartGameResponse(
    Guid GameId,
    Guid HostId,
    string HostName,
    Guid? OpponentId,
    string? OpponentName,
    GameState State,
    Board Board,
    Mark? HostMark,
    Mark NextTurn); 