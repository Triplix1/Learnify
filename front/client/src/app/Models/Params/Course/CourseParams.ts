import { OrderByParams } from "../OrderByParams";
import { PagedListParams } from "../PagedListParams";

export interface CourseParams {
    pagedListParams: PagedListParams;
    orderByParams: OrderByParams;
    search: string;
    authorId: number;
}