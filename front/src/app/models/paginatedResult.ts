import { IPagination } from "./ipagination";

export class PaginatedResult<T> {
    value: T;
    pagination: IPagination;

    constructor(value: T, pagination: IPagination) {
        this.value = value;
        this.pagination = pagination;
    }
}