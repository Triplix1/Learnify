export interface ApiResponseWithData<T> extends ApiResponse {
    data: T;
}

export interface ApiResponse {
    errorMessage: string;
    errorStackTrace: string;
    isSuccess: boolean;
}