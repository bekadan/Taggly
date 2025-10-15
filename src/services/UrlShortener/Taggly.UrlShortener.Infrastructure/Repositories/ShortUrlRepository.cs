using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Taggly.Common.Abstractions.Repositories;
using Taggly.Common.Dynamic;
using Taggly.Common.Paging;
using Taggly.UrlShortener.Application.Interfaces.Repositories;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Infrastructure.Repositories;

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly IAsyncRepository<ShortUrl, Guid> _repository;

    public ShortUrlRepository(IAsyncRepository<ShortUrl, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAsync(u => u.ShortCode.Value == shortCode, cancellationToken: cancellationToken);
    }

    public async Task<bool> ShortCodeExistsAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await _repository.AnyAsync(u => u.ShortCode.Value == shortCode, cancellationToken: cancellationToken);
    }

    // Delegating generic methods to _repository
    public Task<ShortUrl> AddAsync(ShortUrl entity, CancellationToken cancellationToken = default)
        => _repository.AddAsync(entity, cancellationToken);

    public Task<ICollection<ShortUrl>> AddRangeAsync(ICollection<ShortUrl> entities, CancellationToken cancellationToken = default)
        => _repository.AddRangeAsync(entities, cancellationToken);

    public Task<ShortUrl> UpdateAsync(ShortUrl entity, CancellationToken cancellationToken = default)
        => _repository.UpdateAsync(entity, cancellationToken);

    public Task<ICollection<ShortUrl>> UpdateRangeAsync(ICollection<ShortUrl> entities, CancellationToken cancellationToken = default)
        => _repository.UpdateRangeAsync(entities, cancellationToken);

    public Task<ShortUrl> DeleteAsync(ShortUrl entity, bool permanent = false, CancellationToken cancellationToken = default)
        => _repository.DeleteAsync(entity, permanent, cancellationToken);

    public Task<ICollection<ShortUrl>> DeleteRangeAsync(ICollection<ShortUrl> entities, bool permanent = false, CancellationToken cancellationToken = default)
        => _repository.DeleteRangeAsync(entities, permanent, cancellationToken);

    public Task<ShortUrl?> GetAsync(System.Linq.Expressions.Expression<System.Func<ShortUrl, bool>> predicate,
        System.Func<IQueryable<ShortUrl>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ShortUrl, object>>? include = null,
        bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        => _repository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);

    public Task<bool> AnyAsync(System.Linq.Expressions.Expression<System.Func<ShortUrl, bool>>? predicate = null,
        System.Func<IQueryable<ShortUrl>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ShortUrl, object>>? include = null,
        bool withDeleted = false, CancellationToken cancellationToken = default)
        => _repository.AnyAsync(predicate, include, withDeleted, cancellationToken);

    public Task<IPaginate<ShortUrl>> GetListAsync(Expression<Func<ShortUrl, bool>>? predicate = null, Func<IQueryable<ShortUrl>, IOrderedQueryable<ShortUrl>>? orderBy = null, Func<IQueryable<ShortUrl>, IIncludableQueryable<ShortUrl, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IPaginate<ShortUrl>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<ShortUrl, bool>>? predicate = null, Func<IQueryable<ShortUrl>, IIncludableQueryable<ShortUrl, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
