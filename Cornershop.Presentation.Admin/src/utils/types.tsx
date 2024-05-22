export interface ErrorResponse {
  message: string;
  code: string;
  response: {
    data: {
      status: string;
      message: string;
    };
  };
}
