import { makeAutoObservable } from "mobx";
import { IPagination } from "../models/ipagination";
import PagingParams from "../models/pagingParams";

export default class Paging {
    pagination: IPagination | null = null;
    pagingParams = new PagingParams();

    constructor() {
        makeAutoObservable(this);
    }

    setPagingParams = (pagingParams: PagingParams) => {
        this.pagingParams = pagingParams;
    }

    get pageParams() {
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber === 0 ? '1'
            : this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        return params;
    }

    setPagination = (pagination: IPagination) => {
        this.pagination = pagination;
    }
}