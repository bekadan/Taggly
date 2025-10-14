using Taggly.Exceptions.Base;

namespace Taggly.Exceptions.Helpers;

public static class ValidationHelper
{
    public static void ThrowIfInvalid(Dictionary<string, string[]> errors)
    {
        if (errors != null && errors.Count > 0)
            throw new ValidationException(errors);
    }
}
