namespace E_Commerce.Domain.Shared;

public class Result<T> 
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; private set; } = null!;
    public T? Value { get; private set; }

    private Result(bool isSuccess, Error error, T? value)
    {
        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(true, Error.None, value);
    public static Result<T> Failure(Error error) => new Result<T>(false, error, default);
}
