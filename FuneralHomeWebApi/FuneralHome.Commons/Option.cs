namespace FuneralHome.Commons;

/// <summary>
/// Optional data type. 
/// </summary>
/// <typeparam name="TData"></typeparam>
public struct Option<TData>
{
    private readonly TData? _data;
    private readonly bool _isSome;

    /// <summary>
    /// Is this Some data?
    /// </summary>
    public bool IsSome => _isSome;

    /// <summary>
    /// Is this None?
    /// </summary>
    public bool IsNone => !_isSome;

    /// <summary>
    /// Optional data - check <see cref="IsSome"/> before use!
    /// </summary>
    public TData Data => _data!;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="data">Possible data or null</param>
    /// <param name="isSome">Is there data? Set to false on null</param>
    internal Option(TData? data, bool isSome)
    {
        _data = data;
        _isSome = data != null && isSome; // don't set Some to true no matter what if data is null
    }

    /// <summary>
    /// Implicit operator to evaluate Option[T] as boolean
    /// </summary>
    /// <param name="option">Option being evaluated</param>
    public static implicit operator bool(Option<TData> option) => option.IsSome;

    public override bool Equals(object? obj)
    {
        return obj is Option<TData> option &&
               EqualityComparer<TData?>.Default.Equals(_data, option._data) &&
               _isSome == option._isSome;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_data, _isSome);
    }
}

/// <summary>
/// Option creation and extension methods
/// </summary>
public static class Options
{
    /// <summary>
    /// Create Some Option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">Data in the option. Null values evaluate to None</param>
    /// <returns>Option[T] that is Some, but None if data is null</returns>
    public static Option<T> Some<T>(T? data)
        => new Option<T>(data, true);

    /// <summary>
    /// Create None Option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Option[T] that is None</returns>
    public static Option<T> None<T>()
        => new Option<T>(default, false);

    #region BLACK MAGIC

    /// <summary>
    /// Map high-order function (fmap). Maps the data inside the option via the mapping function. 
    /// This makes Option[T] a functor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="targetOption">Option[T] on which Map is called</param>
    /// <param name="mappingFunction">Func to map the data inside the Option (T -> R)</param>
    /// <returns>Option[R] as either Some or None</returns>
    public static Option<R> Map<T, R>(this Option<T> targetOption, Func<T, R> mappingFunction)
        => targetOption
            ? Options.Some(mappingFunction(targetOption.Data))
            : Options.None<R>();

    /// <summary>
    /// Bind high-order function (>>=). Maps the data inside the option to another option. Used to cascade Option-returning operations. 
    /// This makes Option[T] a monad. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="targetOption">Option[T] on which Bind is called</param>
    /// <param name="bindingFunction">Func to map the data inside the Option to a new Option (T -> Option[R])</param>
    /// <returns>Option[R] as either Some or None</returns>
    public static Option<R> Bind<T, R>(this Option<T> targetOption, Func<T, Option<R>> bindingFunction)
        => targetOption
            ? bindingFunction(targetOption.Data)
            : Options.None<R>();

    /// <summary>
    /// Async bind high-order function (>>=). Maps the data inside the option to another option. Used to cascade Option-returning operations. 
    /// This makes Option[T] a monad. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="targetOption">Option[T] on which Bind is called</param>
    /// <param name="bindingFunction">Func to map the data inside the Option to a new Option (T -> Option[R])</param>
    /// <returns>Option[R] as either Some or None</returns>
    public static async Task<Option<R>> Bind<T, R>(this Task<Option<T>> targetOption, Func<T, Task<Option<R>>> bindingFunction)
        => await targetOption
            ? await bindingFunction((await targetOption).Data)
            : Options.None<R>();

    #endregion
}
