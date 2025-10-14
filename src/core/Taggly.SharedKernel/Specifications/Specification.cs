using System.Linq.Expressions;

namespace Taggly.SharedKernel.Specifications;

/// <summary>
/// Base helper for creating specifications.
/// </summary>
public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T candidate)
    {
        var predicate = ToExpression().Compile();
        return predicate(candidate);
    }

    public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
    public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
    public ISpecification<T> Not() => new NotSpecification<T>(this);

    public virtual IQueryable<T> Apply(IQueryable<T> query) => query.Where(ToExpression());
}

internal sealed class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;
    public AndSpecification(ISpecification<T> left, ISpecification<T> right) { _left = left; _right = right; }
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();
        return leftExpr.Compose(rightExpr, Expression.AndAlso);
    }
}

internal sealed class OrSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;
    public OrSpecification(ISpecification<T> left, ISpecification<T> right) { _left = left; _right = right; }
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();
        return leftExpr.Compose(rightExpr, Expression.OrElse);
    }
}

internal sealed class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _spec;
    public NotSpecification(ISpecification<T> spec) { _spec = spec; }
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = _spec.ToExpression();
        var param = expr.Parameters.Single();
        var body = Expression.Not(expr.Body);
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

internal static class ExpressionExtensions
{
    public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        var map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    private sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map) => _map = map ?? new();

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            => new ParameterRebinder(map).Visit(exp);

        protected override Expression VisitParameter(ParameterExpression node)
            => _map.TryGetValue(node, out var replacement) ? replacement : base.VisitParameter(node);
    }
}
