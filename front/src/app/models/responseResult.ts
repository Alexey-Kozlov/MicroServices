
export class ResponseResult<T> {
    isSuccess: boolean;
    errors: number[];
    message: string;
    result: T;

    constructor(isSuccess: boolean, errors: number[], message: string, result: T) {
        this.isSuccess = isSuccess;
        this.errors = errors;
        this.message = message;
        this.result = result;
    }

}