// apiService.js
import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept-Language': 'en-US'
  },
  withCredentials: true
});

export const get = (url : string, config = {}) => api.get(url, config);
export const post = (url: string, data: object, config = {}) => api.post(url, data, config);
export const put = (url: string, data: object, config = {}) => api.put(url, data, config);
export const patch = (url: string, data: object, config = {}) => api.patch(url, data, config);
export const del = (url: string, config = {}) => api.delete(url, config);

