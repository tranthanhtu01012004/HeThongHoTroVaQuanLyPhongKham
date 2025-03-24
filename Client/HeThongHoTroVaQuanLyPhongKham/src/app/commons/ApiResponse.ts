export interface ApiResponse<T> {
    status: boolean;
    message?: string;
    data?: T;
    page?: number;
    pageSize?: number;
    totalPages?: number;
    totalItems?: number;
}