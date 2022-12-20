namespace OrdersAPI.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string ErrorText { get; set; }
        public ErrorEnum ErrorType { get; set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        public static Result<T> Error(ErrorEnum errorEnum, string errorText) => new Result<T>
        {
            IsSuccess = false,
            ErrorText = errorText,
            ErrorType = errorEnum
        };
    }

    public enum ErrorEnum
    {
        NotFoundError,
        ApplicationError
    }
}
