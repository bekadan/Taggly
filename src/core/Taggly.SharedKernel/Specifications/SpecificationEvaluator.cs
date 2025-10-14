namespace Taggly.SharedKernel.Specifications;

/// <summary>
/// Applies specification to IQueryable sources.
/// </summary>
public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T>? specification)
    {
        if (specification == null) return inputQuery;
        return specification.Apply(inputQuery);
    }
}
