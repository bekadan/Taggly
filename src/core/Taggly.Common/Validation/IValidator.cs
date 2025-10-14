using Taggly.Common.Types;

namespace Taggly.Common.Validation;

public interface IValidator<T>
    where T : class
{
    /// <summary>
    /// Sets the next validator in the chain.
    /// </summary>
    /// <param name="next">The next validator instance.</param>
    /// <returns>The initial validator instance.</returns>
    IValidator<T> SetNext(IValidator<T> next);

    /// <summary>
    /// Validates the specified item.
    /// </summary>
    /// <param name="item">The item that is being validated.</param>
    /// <returns>The result instance representing the success status of the validation.</returns>
    Result Validate(T? item);
}
