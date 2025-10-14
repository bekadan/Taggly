using System.Linq.Expressions;

namespace Taggly.SharedKernel.Specifications;

/// <summary>
/// Encapsulates a predicate and query configuration for a given type.
/// </summary>
public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
    bool IsSatisfiedBy(T candidate);
    ISpecification<T> And(ISpecification<T> other);
    ISpecification<T> Or(ISpecification<T> other);
    ISpecification<T> Not();
    IQueryable<T> Apply(IQueryable<T> query);
}
