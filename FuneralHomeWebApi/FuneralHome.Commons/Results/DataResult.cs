namespace FuneralHome.Commons.Results;

/// <summary>
/// Result data type with underlying data
/// </summary>
/// <typeparam name="TData"></typeparam>
public struct Result<TData>
{
    private readonly TData? _data;
    private readonly string _message;
    private readonly ResultTypes _resultType;
    private readonly Exception? _exception;

    /// <summary>
    /// Is the operation a succcess?
    /// </summary>
    public bool IsSuccess => _data != null && _resultType == ResultTypes.SUCCESS;
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
    /// Has the result any data?
    /// </summary>
    public bool HasData => _data != null;
    /// <summary>
    /// Result type
    /// </summary>
    public ResultTypes ResultType => _resultType;
    /// <summary>
    /// Result data - don't use unless result type is SUCCESS
    /// </summary>
    public TData Data => _data!;
    /// <summary>
    /// Thrown exception - don't use unless result type is EXCEPTION
    /// </summary>
    public Exception Exception => _exception!;
    /// <summary>
    /// Result message
    /// </summary>
    public string Message => _message;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="data">Possible data</param>
    /// <param name="resultType">Result type</param>
    /// <param name="message">Result message</param>
    /// <param name="exception">Possible exception</param>
    internal Result(TData? data, ResultTypes resultType, string? message = null, Exception? exception = null)
    {
        _data = data;
        _exception = exception;

        // check the result type against the data just in case
        _resultType = resultType == ResultTypes.SUCCESS && _data == null ? ResultTypes.FAILURE : resultType;
        // if no exception is given then, it's a failure - just in case
        _resultType = resultType == ResultTypes.EXCEPTION && _exception == null ? ResultTypes.FAILURE : _resultType;

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
    public static implicit operator bool(Result<TData> result)
        => result.IsSuccess;

    public override bool Equals(object? obj)
    {
        return obj is Result<TData> result &&
               EqualityComparer<TData?>.Default.Equals(_data, result._data) &&
               _message == result._message &&
               _resultType == result._resultType &&
               EqualityComparer<Exception?>.Default.Equals(_exception, result._exception);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_data, _message, _resultType, _exception);
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
    /// <typeparam name="T"></typeparam>
    /// <param name="data">Data</param>
    /// <param name="message">Custom success message</param>
    /// <returns>Successful Result[T] with data</returns>
    public static Result<T> OnSuccess<T>(T data, string? message = null)
        => new Result<T>(data, ResultTypes.SUCCESS, message);

    /// <summary>
    /// Create failure result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message">Custom failure message</param>
    /// <returns>Failure Result[T]</returns>
    public static Result<T> OnFailure<T>(string? message = null)
        => new Result<T>(default, ResultTypes.FAILURE, message); // default give null for ref types

    /// <summary>
    /// Create exception result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="exception">Thrown exception during operation</param>
    /// <param name="message">Custom message</param>
    /// <returns>Exception Result[T]</returns>
    public static Result<T> OnException<T>(Exception exception, string? message = null)
        => new Result<T>(default, ResultTypes.EXCEPTION, message, exception);

    #region BLACK MAGIC

    /// <summary>
    /// Map high-order function for Result[T]. This makes Result[T] a functor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="targetResult">Result[T] on which Map is called</param>
    /// <param name="mappingFunction">Mapping function (T -> R)</param>
    /// <returns>Result[R] as either success, failure, or exception</returns>
    public static Result<R> Map<T, R>(this Result<T> targetResult, Func<T, R> mappingFunction)
    => targetResult switch
    {
        { ResultType: ResultTypes.SUCCESS } => Results.OnSuccess(mappingFunction(targetResult.Data)),
        { ResultType: ResultTypes.FAILURE } => Results.OnFailure<R>(targetResult.Message),
        { ResultType: ResultTypes.EXCEPTION } => Results.OnException<R>(targetResult.Exception, targetResult.Message),
        _ => Results.OnFailure<R>("Unknown result type")
    };

    /// <summary>
    /// Bind high-order function for Result[T]. This makes Result[T] a monad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="targetResult">Result[T] on which Bind is called</param>
    /// <param name="mappingFunction">Binding function (T -> Result[R])</param>
    /// <returns>Result[R] as either success, failure, or exception</returns>
    public static Result<R> Bind<T, R>(this Result<T> targetResult, Func<T, Result<R>> bindingFunction)
        => targetResult switch
        {
            { ResultType: ResultTypes.SUCCESS } => bindingFunction(targetResult.Data),
            { ResultType: ResultTypes.FAILURE } => Results.OnFailure<R>(targetResult.Message),
            { ResultType: ResultTypes.EXCEPTION } => Results.OnException<R>(targetResult.Exception, targetResult.Message),
            _ => Results.OnFailure<R>("Unknown result type")
        };

    /// <summary>
    /// Async bind high-order function for Result[T]. This makes Result[T] a monad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="targetResult">Result[T] on which Bind is called</param>
    /// <param name="mappingFunction">Binding function (T -> Result[R])</param>
    /// <returns>Result[R] as either success, failure, or exception</returns>
    public static async Task<Result<R>> Bind<T, R>(this Task<Result<T>> targetResult, Func<T, Task<Result<R>>> bindingFunction)
        => await targetResult switch
        {
            { ResultType: ResultTypes.SUCCESS } => await bindingFunction((await targetResult).Data),
            { ResultType: ResultTypes.FAILURE } => Results.OnFailure<R>((await targetResult).Message),
            { ResultType: ResultTypes.EXCEPTION } => Results.OnException<R>((await targetResult).Exception, (await targetResult).Message),
            _ => Results.OnFailure<R>("Unknown result type")
        };

    /// <summary>
    /// Bind on Result[T] that accepts Result as result 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetResult">Target result on which bind is called</param>
    /// <param name="bindingFunction">Binding function (T -> Result)</param>
    /// <returns>Result</returns>
    public static Result Bind<T>(this Result<T> targetResult, Func<T, Result> bindingFunction)
        => targetResult switch
        {
            { ResultType: ResultTypes.SUCCESS } => bindingFunction(targetResult.Data),
            { ResultType: ResultTypes.FAILURE } => Results.OnFailure(targetResult.Message),
            { ResultType: ResultTypes.EXCEPTION } => Results.OnException(targetResult.Exception, targetResult.Message),
            _ => Results.OnFailure("Unknown result type")
        };

    /// <summary>
    /// Async bind on Result[T] that accepts Result as result 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetResult">Target result on which bind is called</param>
    /// <param name="bindingFunction">Binding function (T -> Result)</param>
    /// <returns>Result</returns>
    public static async Task<Result> Bind<T>(this Task<Result<T>> targetResult, Func<T, Task<Result>> bindingFunction)
        => await targetResult switch
        {
            { ResultType: ResultTypes.SUCCESS } => await bindingFunction((await targetResult).Data),
            { ResultType: ResultTypes.FAILURE } => Results.OnFailure((await targetResult).Message),
            { ResultType: ResultTypes.EXCEPTION } => Results.OnException((await targetResult).Exception, (await targetResult).Message),
            _ => Results.OnFailure("Unknown result type")
        };
    #endregion 
}