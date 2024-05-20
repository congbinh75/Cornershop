import axios from "axios";
import { handleResponse, handleError } from "./response";

const BASE_URL = import.meta.env.VITE_BASE_URL;

export const getAll = async (resource: string) => {
  try {
    const response = await axios.get(`${BASE_URL}/${resource}`);
    return handleResponse(response);
  } catch (error) {
    return handleError(error);
  }
};

export const getSingle = async (resource: string, id: string) => {
  try {
    const response = await axios.get(`${BASE_URL}/${resource}/${id}`);
    return handleResponse(response);
  } catch (error) {
    return handleError(error);
  }
};

export const post = async (resource: string, model: object) => {
  try {
    const response = await axios.post(`${BASE_URL}/${resource}`, model);
    return handleResponse(response);
  } catch (error) {
    return handleError(error);
  }
};

export const put = async (resource: string, model: object) => {
  try {
    const response = await axios.put(`${BASE_URL}/${resource}`, model);
    return handleResponse(response);
  } catch (error) {
    return handleError(error);
  }
};

export const patch = async (resource: string, model: object) => {
  try {
    const response = await axios.patch(`${BASE_URL}/${resource}`, model);
    return handleResponse(response);
  } catch (error) {
    return handleError(error);
  }
};

export const remove = async (resource: string, id: string) => {
  try {
    const response = await axios.delete(`${BASE_URL}/${resource}/${id}`);
    return handleResponse(response);
  } catch (error) {
    return handleError(error);
  }
};
