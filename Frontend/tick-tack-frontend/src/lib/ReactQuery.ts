/* eslint-disable @typescript-eslint/no-explicit-any */
import { DefaultError, DefaultOptions, QueryClient, QueryKey, UseInfiniteQueryOptions, UseMutationOptions, UseQueryOptions } from "@tanstack/react-query";
import { AsyncReturnType } from 'type-fest';

const queryConfig: DefaultOptions = {
  queries: {
    refetchOnWindowFocus: false,
    retry: false
  },
};

export const queryClient = new QueryClient({ defaultOptions: queryConfig });

export type ExtractFnReturnType<FnType extends (...args: any) => any> = AsyncReturnType<FnType>;

export type QueryConfig<T> = Omit<
  UseQueryOptions<T>,
  'queryKey' | 'queryFn'
>;

export type InfiniteQueryConfig<TQueryFnData, TError = DefaultError, TData = TQueryFnData, TQueryData = TQueryFnData, TQueryKey extends QueryKey = QueryKey, TPageParam = unknown> = Omit<
  UseInfiniteQueryOptions<TQueryFnData, TError, TData, TQueryData, TQueryKey, TPageParam>,
  'queryKey' | 'queryFn'
>;

export type MutationConfig<MutationFnType extends (...args: any) => any> = UseMutationOptions<
  ExtractFnReturnType<MutationFnType>,
  unknown,
  Parameters<MutationFnType>[0]
>;