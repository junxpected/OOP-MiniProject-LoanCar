namespace LoanCar.Domain
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; } // Додали '?'
        public string? ErrorMessage { get; } // Додали '?'

        private Result(bool isSuccess, T? value, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
    }
}