import useSWR from 'swr';
import { get, post, put, del } from './core';

const fetcher = (url: string) => get(url).then(res => res.data);

export const useGet = (url: string) => {
  const { data, error, mutate } = useSWR(url, fetcher);
  return {
    data,
    isLoading: !error && !data,
    isError: error,
    mutate,
  };
};

export const usePost = (url: string, data: object) => {
  return post(url, data);
};

export const usePut = (url: string, data: object) => {
  return put(url, data);
};

export const useDelete = (url: string) => {
  return del(url);
};
