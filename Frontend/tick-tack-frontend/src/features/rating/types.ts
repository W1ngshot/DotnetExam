export interface PlayerInfoDto
{
  id: string,
  username: string,
  rating: number,
  mark: MarkEnum
}

export interface GameInfoDto{
  gameId: string,
  createdAt: string,
  state: GameStateEnum,
  host: PlayerInfoDto,
  opponent: PlayerInfoDto,
}

export enum GameStateEnum
{
    NotStarted,
    Started,
    NoughtsWon,
    CrossesWon,
    Draw
}

export enum MarkEnum
{
    Cross = 0,
    Nough = 1
}