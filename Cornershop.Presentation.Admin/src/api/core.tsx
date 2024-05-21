// apiService.js
import axios from 'axios';
import { toast } from 'react-toastify';

const api = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept-Language': 'en-US'
  },
  withCredentials: true
});

const handleResponse = (response) => response.data;

const handleError = (error) => {
  const message = error.response?.data?.message || error.message;
  toast.error(message);
  return Promise.reject(error);
};

api.interceptors.response.use(handleResponse, handleError);

export const get = (url : string, config = {}) => api.get(url, config);
export const post = (url: string, data: object, config = {}) => api.post(url, data, config);
export const put = (url: string, data: object, config = {}) => api.put(url, data, config);
export const del = (url: string, config = {}) => api.delete(url, config);
