import { toast } from "react-toastify";
import { failure } from "../../utils/constants";

interface Response {
  config: object;
  data: {
    data: object;
    status: string;
    message: string;
  };
  headers: object;
  request: string;
  status: number;
  statusText: string;
}

interface Error {
  stack: string;
  message: string;
  name: string;
  config: object;
  code: string;
  request: string;
  response: {
    data: {
      status: string;
      message: string;
    }
  };
}

export function handleResponse(response: Response) {
  if (response.data) {
    if (response.data.status == failure) {
      toast.error(response.data.message);
    }
    return response.data;
  }
  return response;
}

export function handleError(error: Error) {
  if (error.response?.data?.message) {
      toast.error(error.response.data.message);
      return error.response.data.message;
  }
  if (error.message) {
    toast.error(error.message);
    return error.message;
  }
  return error;
}
