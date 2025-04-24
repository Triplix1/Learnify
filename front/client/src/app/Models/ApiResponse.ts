export interface ApiResponseWithData<T> extends ApiResponseWithDataAndError<T, void> {
}

export interface ApiResponseWithDataAndError<T, TError> extends ApiResponse {
    data: T;
    errorData: TError;
}

export interface ApiResponse {
    errorMessage: string;
    errorStackTrace: string;
    isSuccess: boolean;
}