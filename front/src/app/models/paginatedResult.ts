import { IPagination } from "./ipagination";

export class PaginatedResult<T> {
    Items: T;
    pagination: IPagination;

    constructor(Items: T, pagination: IPagination) {
        this.Items = Items;
        this.pagination = pagination;
    }
}