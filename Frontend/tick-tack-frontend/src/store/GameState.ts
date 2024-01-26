import { GameOverEvent, GameStartEvent, PlayerMoveEvent, SendMessageEvent } from './../features/game/gameApi';
import { GameStateEnum, MarkEnum, PlayerInfoDto } from "@/features/rating/types";
import { action, computed, makeObservable, observable } from "mobx";
import { nanoid } from 'nanoid';

export class Player {
  public readonly id: string;
  public readonly playerMark: MarkEnum;
  public readonly name: string;
  public readonly score: number;

  constructor(dto: PlayerInfoDto) {
    this.id = dto.id;
    this.name = dto.username;
    this.score = dto.rating;
    this.playerMark = dto.mark;
    makeObservable(this);
  }

  @observable
  private _isMoving: boolean = false;

  @computed
  public get isMoving(): boolean {
    return this._isMoving;
  }
  public set isMoving(v: boolean) {
    this._isMoving = v;
  }
}

export class GameState {
  public readonly host: Player;
  public readonly idempotenceKey = nanoid();
  public readonly gameId: string;

  constructor(
    public readonly localUserId: string,
    public readonly sizeX: number,
    public readonly sizeY: number,
    public readonly isGuest: boolean,
    startEvent: GameStartEvent) {
    this.gameId = startEvent.gameId;
    this.host = new Player(startEvent.host);
    this.handleStart(startEvent);
    makeObservable(this);
  }

  @computed
  public get canMove(): boolean {
    return !this.isGuest && this.localPlayer.isMoving;
  }

  @observable
  private _opponent: Player | undefined;

  @computed
  public get opponent(): Player | undefined {
    return this._opponent;
  }

  public set opponent(v: Player | undefined) {
    this._opponent = v;
  }

  @computed
  public get localPlayer(): Player {
    return this.host.id === this.localUserId ? this.host : this._opponent!;
  }

  @observable
  private _gameStatus: GameStateEnum = GameStateEnum.NotStarted;

  @computed
  public get gameStatus(): GameStateEnum {
    return this._gameStatus;
  }
  public set gameStatus(v: GameStateEnum) {
    this._gameStatus = v;
  }

  @observable
  private _gameField: string[];

  @computed
  public get field(): string[] {
    return this._gameField;
  }

  public set field(x: string[]) {
    this._gameField = x;
  }

  @observable
  private _winner: Player | undefined;

  @computed
  public get winner(): Player | undefined {
    return this._winner;
  }
  public set winner(v: Player | undefined) {
    this._winner = v;
  }

  @action
  public handleStart(startEvent: GameStartEvent) {
    this._gameField = startEvent.board;
    if (startEvent.opponent) {
      this._opponent = new Player(startEvent.opponent);
      this.makeHostTurn(startEvent.currentTurnId == this.host.id)
    }
  }

  @action
  public handleMove(startEvent: PlayerMoveEvent) {
    if (startEvent.idempotenceKey === this.idempotenceKey) {
      return;
    }
    this._gameField = startEvent.board;
    this.makeHostTurn(startEvent.currentTurnId == this.host.id)
  }

  @action
  public handleEnd(event: GameOverEvent) {
    if (this._opponent) {
      this._opponent.isMoving = false;
    }
    this.host.isMoving = false;
    this._gameStatus = event.gameState;
    if (this.host.id == event.winnerId) {
      this.winner = this.host;
    } else {
      this.winner = this.opponent;
    }
  }

  @action
  private makeHostTurn(hostTurn: boolean) {
    if (!this._opponent) {
      return;
    }

    this._opponent.isMoving = !hostTurn;
    this.host.isMoving = hostTurn;
  }
}

export interface Message {
  message: string,
  sender: PlayerInfoDto,
}

export class ChatController {
  public readonly idempotenmcyKey = nanoid();
  constructor() {
    makeObservable(this)
  }

  @observable
  private _messages: Message[] = [];
  @computed
  public get messages(): Message[] {
    return this._messages;
  }

  @action
  public addMessage(msg: SendMessageEvent) {
    if(msg.idempotenceKey === this.idempotenmcyKey){
      return;
    }

    this._messages = [...this._messages, {
      message: msg.message,
      sender: msg.sender
    }];
  }
}