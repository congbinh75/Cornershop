export interface SuccessResponse {
    config: object;
    data: {
      status: string;
      message: string;
    };
    headers: object;
    request: string;
    status: number;
    statusText: string;
  }
  
export type ErrorResponse = {
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

export type loginResponse = {
    status: string;
    message: string;
}