import { apiClient } from "@/lib/ApiClient";
import { GameInfoDto } from "./types";
import { DefaultError, InfiniteData, QueryKey, useInfiniteQuery, useMutation } from "@tanstack/react-query";
import { MutationConfig } from "@/lib/ReactQuery";
import { toast } from "react-toastify";

export interface CreateGameRequest {
  maxRating: number;
}

export interface CreateGameResponse {
  gameId: string;
}

const createGame = (data: CreateGameRequest): Promise<CreateGameResponse> => {
  return apiClient.post("game/create", { json: data }).json<CreateGameResponse>();
};

type UseCreateGame = {
  config?: MutationConfig<typeof createGame>;
};

export const useCreateGame = ({ config }: UseCreateGame) => {
  return useMutation({
    onError: (err, __, context: any) => {
      toast("Ошибка создания", { type: "error" });
    },
    onSuccess: (data) => {
    },
    ...config,
    mutationFn: createGame,
  });
};

interface GetGameListRequest extends Record<string, number | string> {
  skip: number;
  count: number;
}

export interface GetGameListResponse {
  games: GameInfoDto[];
}

export function getGameList(request: GetGameListRequest) {
  return apiClient.get('game', { searchParams: request }).json<GetGameListResponse>();
}

export const useGetGameList = (pageSize: number) => {
  return useInfiniteQuery<GetGameListResponse, DefaultError, InfiniteData<GetGameListResponse>, QueryKey, number>({
    initialPageParam: 0,
    getNextPageParam: (lp, u2, x: number) => lp.games.length > pageSize ? x + pageSize : null,
    queryKey: ["game-list"],
    queryFn: ({ pageParam }) => getGameList({skip: pageParam * pageSize, count: pageSize}),
  });
}

interface GameRatingRequest{
  count: number;
}

interface GameRatingResponseItem{
  rating: number;
  username: string;
}

interface GameRatingResponse {
  player: GameRatingResponseItem[]
}

const getRating = (data: GameRatingRequest): Promise<GameRatingResponse> => {
  return apiClient.post("game/rating", { searchParams: data }).json<GameRatingResponse>();
};

type UseGetRating = {
  count: number;
  config?: MutationConfig<typeof getRating>;
};

export const useGetRating = ({ config, count }: UseGetRating) => {
  return useMutation({
    onError: (err, __, context: any) => {
      toast("Ошибка создания", { type: "error" });
    },
    onSuccess: (data) => {
    },
    ...config,
    mutationFn: getRating,
  });
};
