export interface PagedList<T> {
    items: T[];
    currentPage: number;
    pageSize: number;
    totalCount: number;
}