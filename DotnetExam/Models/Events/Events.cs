﻿using DotnetExam.Models.Enums;

namespace DotnetExam.Models.Events;

public record GameStartEvent(
    Guid GameId,
    PlayerInfo Host,
    PlayerInfo Opponent,
    string[] Board,
    Guid CurrentTurnId,
    GameState State);

public record PlayerMoveEvent(Guid GameId, string IdempotenceKey, string[] Board, Guid CurrentTurnId);

public record GameOverEvent(Guid GameId, string[] Board, Guid? WinnerId, GameState State);

public record SendMessageEvent(Guid GameId, string IdempotenceKey, string Message, UserInfo Sender);

public record GameRestartEvent(Guid OldGameId, Guid GameId, PlayerInfo Host, PlayerInfo Opponent);

public record GameClosedEvent(Guid GameId);