import { MutationConfig } from "@/lib/ReactQuery";
import { GameStateEnum, PlayerInfoDto } from "../rating/types";
import { apiClient } from "@/lib/ApiClient";
import { ChatController, GameState } from "@/store/GameState";
import { useMutation } from "@tanstack/react-query";
import { toast } from "react-toastify";

export interface GameStartEvent {
  gameId: string;
  host: PlayerInfoDto;
  opponent: PlayerInfoDto;
  board: string[],
  currentTurnId: string,
}

export interface PlayerMoveEvent {
  gameId: string;
  idempotenceKey: string;
  board: string[];
  currentTurnId: string;
}

export interface GameOverEvent {
  gameId: string;
  board: string[];
  winnerId: string;
  gameState: GameStateEnum
}

export interface GameRestartEvent {
  gameId: string;
  host: PlayerInfoDto;
  opponent: PlayerInfoDto
}

export interface SendMessageEvent {
  gameId: string;
  idempotenceKey: string;
  sender: PlayerInfoDto;
  message: string;
}

export enum JoinModeEnum {
  AsPlayer,
  AsViewer
}

export interface JoinRequest {
  gameId: string;
  joinMode: JoinModeEnum
}

const joinGame = (data: JoinRequest): Promise<GameStartEvent> => {
  return apiClient.post("game/join", { json: data }).json<GameStartEvent>();
};

type UseJoinGame = {
  config?: MutationConfig<typeof joinGame>;
};

export const useJoinGame = ({ config }: UseJoinGame) => {
  return useMutation({
    onError: (err, __, context: any) => {
    },
    onSuccess: (data) => {
    },
    ...config,
    mutationFn: joinGame,
  });
};

export interface MoveRequest{
  x: number,
  y: number,
  idempotenceKey?: string
}

const makeMove = (data: MoveRequest): Promise<PlayerMoveEvent> => {
  return apiClient.post("game/move", { json: data }).json<PlayerMoveEvent>();
};

type UseMakeMove = {
  gameState: GameState,
  config?: MutationConfig<(data: Omit<MoveRequest, "idempotenceKey">) => Promise<PlayerMoveEvent>>;
};

export const useMakeMove = ({ gameState, config }: UseMakeMove) => {
  return useMutation({
    onError: (err, __, context: any) => {
      toast("Ошибка хода", { type: "error" });
    },
    onSuccess: (data) => {
      data.idempotenceKey = '';
      gameState.handleMove(data);
    },
    ...config,
    mutationFn: async (x) => {
      return await makeMove({
        ...x,
        idempotenceKey: gameState.idempotenceKey,
      })
    }
  });
};

export interface SendMessageRequest{
  message: string;
  gameId: string;
  idempotenceKey: string;
}

const sendMessage = (data: SendMessageRequest): Promise<SendMessageEvent> => {
  return apiClient.post("game/send", { json: data }).json<SendMessageEvent>();
};

type UseSendMessage = {
  chatController: ChatController,
  gameState: GameState,
  config?: MutationConfig<((data: Pick<SendMessageRequest, "message">) =>Promise<SendMessageEvent>)>;
};

export const useSendMessage = ({ gameState, chatController, config }: UseSendMessage) => {
  return useMutation({
    onError: (err) => {
      toast("Ошибка отправки сообщений", { type: "error" });
    },
    onSuccess: (data) => {
      data.idempotenceKey = '';
      chatController.addMessage(data);
    },
    ...config,
    mutationFn: async (x) => {
      return await sendMessage({
        ...x,
        idempotenceKey: chatController.idempotenmcyKey,
        gameId: gameState.gameId,
      })
    }
  });
};