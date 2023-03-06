namespace FuneralHome.Commons.Results;
/// <summary>
/// Result data type without data
/// </summary>
public struct Result
{
    private readonly string _message;
    private readonly ResultTypes _resultType;
    private readonly Exception? _exception;

    /// <summary>
    /// Is the operation a succcess?
    /// </summary>
    public bool IsSuccess => _resultType == ResultTypes.SUCCESS;
    /// <summary>
    /// Is the operation a failure?
    /// </summary>
    public bool IsFailure => _resultType == ResultTypes.FAILURE;
    /// <summary>
    /// Did the operation throw an exception?
    /// </summary>
    public bool IsException => _resultType == ResultTypes.EXCEPTION;
    /// <summary>
    /// Has the result an exception?
    /// </summary>
    public bool HasException => _exception != null;

    /// <summary>
    /// Result type
    /// </summary>
    public ResultTypes ResultType => _resultType;
    /// <summary>
    /// Possible exception - don't use unless result type is EXCEPTION
    /// </summary>
    public Exception Exception => _exception!;
    /// <summary>
    /// Result message
    /// </summary>
    public string Message => _message;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="resultType">Result type</param>
    /// <param name="message">Result message</param>
    /// <param name="exception">Possible exception</param>
    internal Result(ResultTypes resultType, string? message = null, Exception? exception = null)
    {
        _exception = exception;
        // if no exception is given then, it's a failure - just in case
        _resultType = resultType == ResultTypes.EXCEPTION && _exception == null ? ResultTypes.FAILURE : resultType;

        // if no message is given, set the message according to the result type
        _message = message ?? resultType switch
        {
            ResultTypes.SUCCESS => "Operation successful",
            ResultTypes.FAILURE => "Operation failed",
            ResultTypes.EXCEPTION => $"Operation failed with exception: {exception!.Message}", // can't be null at this point; takes exception message
            _ => "UNKNOWN" // never gets to this, just to stop the warn
        };
    }

    /// <summary>
    /// Implicit bool operator to evaluate Result[T] as boolean
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator bool(Result result)
        => result.IsSuccess;

    public override bool Equals(object? obj)
    {
        return obj is Result result &&
               _message == result._message &&
               _resultType == result._resultType &&
               EqualityComparer<Exception?>.Default.Equals(_exception, result._exception);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_message, _resultType, _exception);
    }
}

/// <summary>
/// Result creation and extension methods
/// </summary>
public static partial class Results
{
    /// <summary>
    /// Create success result
    /// </summary>
    /// <param name="message">Custom success message</param>
    /// <returns>Successful Result with data</returns>
    public static Result OnSuccess(string? message = null)
        => new Result(ResultTypes.SUCCESS, message);

    /// <summary>
    /// Create failure result
    /// </summary>
    /// <param name="message">Custom failure message</param>
    /// <returns>Failure Result</returns>
    public static Result OnFailure(string? message = null)
        => new Result(ResultTypes.FAILURE, message); // default give null for ref types

    /// <summary>
    /// Create exception result
    /// </summary>
    /// <param name="exception">Thrown exception during operation</param>
    /// <param name="message">Custom message</param>
    /// <returns>Exception Result</returns>
    public static Result OnException(Exception exception, string? message = null)
        => new Result(ResultTypes.EXCEPTION, message, exception);

    #region BLACK MAGIC

    /// <summary>
    /// Bind function for Result
    /// </summary>
    /// <param name="targetResult">Target Result over which Bind is called</param>
    /// <param name="bindingFunction">Binding function (() -> Result)</param>
    /// <returns>Result as either success, failure, or exception</returns>
    public static Result Bind(this Result targetResult, Func<Result> bindingFunction)
        => targetResult switch
        {
            { ResultType: ResultTypes.SUCCESS } => bindingFunction(),
            { ResultType: ResultTypes.FAILURE } => Results.OnFailure(targetResult.Message),
            { ResultType: ResultTypes.EXCEPTION } => Results.OnException(targetResult.Exception, targetResult.Message),
            _ => Results.OnFailure("Unknown result type")
        };

    /// <summary>
    /// Async bind function for Result
    /// </summary>
    /// <param name="targetResult"></param>
    /// <param name="bindingFunction"></param>
    /// <returns>Result as either success, failure, or exception</returns>
    public static async Task<Result> Bind(this Task<Result> targetResult, Func<Task<Result>> bindingFunction)
        => await targetResult switch
        {
            { ResultType: ResultTypes.SUCCESS } => await bindingFunction(),
            { ResultType: ResultTypes.FAILURE } => Results.OnFailure((await targetResult).Message),
            { ResultType: ResultTypes.EXCEPTION } => Results.OnException((await targetResult).Exception, (await targetResult).Message),
            _ => Results.OnFailure("Unknown result type")
        };

    /// <summary>
    /// Result Bind that accepts Result[T] as return type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetResult">Target Result on which Bind is called</param>
    /// <param name="bindingFunction">Binding function (() -> Result[T])</param>
    /// <returns></returns>
    public static Result<T> Bind<T>(this Result targetResult, Func<Result<T>> bindingFunction)
    => targetResult switch
    {
        { ResultType: ResultTypes.SUCCESS } => bindingFunction(),
        { ResultType: ResultTypes.FAILURE } => Results.OnFailure<T>(targetResult.Message),
        { ResultType: ResultTypes.EXCEPTION } => Results.OnException<T>(targetResult.Exception, targetResult.Message),
        _ => Results.OnFailure<T>("Unknown result type")
    };

    /// <summary>
    /// Async result Bind that accepts Result[T] as return type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetResult">Target Result on which Bind is called</param>
    /// <param name="bindingFunction">Binding function (() -> Result[T])</param>
    /// <returns></returns>
    public static async Task<Result<T>> Bind<T>(this Task<Result> targetResult, Func<Task<Result<T>>> bindingFunction)
    => await targetResult switch
    {
        { ResultType: ResultTypes.SUCCESS } => await bindingFunction(),
        { ResultType: ResultTypes.FAILURE } => Results.OnFailure<T>((await targetResult).Message),
        { ResultType: ResultTypes.EXCEPTION } => Results.OnException<T>((await targetResult).Exception, (await targetResult).Message),
        _ => Results.OnFailure<T>("Unknown result type")
    };

    #endregion
}