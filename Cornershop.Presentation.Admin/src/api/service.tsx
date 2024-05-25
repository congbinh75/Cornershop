import useSWR from "swr";
import { get, post, put, del, patch } from "./core";

const fetcher = (url: string) => get(url).then((res) => res.data);

export const useGet = (url: string) => {
  const { data, error, mutate } = useSWR(url, fetcher, {
    revalidateOnFocus: false,
    revalidateOnReconnect: false,
    refreshWhenOffline: false,
    refreshWhenHidden: false,
    refreshInterval: 0,
  });
  return {
    data,
    isLoading: !error && !data,
    error: error,
    mutate,
  };
};

export const usePost = (url: string, data: object) => {
  return post(url, data);
};

export const usePut = (url: string, data: object) => {
  return put(url, data);
};

export const usePatch = (url: string, data: object) => {
  return patch(url, data);
};

export const useDelete = (url: string) => {
  return del(url);
};
