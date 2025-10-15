using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Taggly.Common.Domain;
using Taggly.Common.Dynamic;
using Taggly.Common.Paging;

namespace Taggly.Common.Abstractions.Repositories
{
    public class AsyncRepositoryBase<TEntity, TContext> : IAsyncRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public AsyncRepositoryBase(TContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.CreatedOnUtc = DateTime.UtcNow;
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {

            foreach (TEntity entity in entities)
                entity.CreatedOnUtc = DateTime.UtcNow;
            await _context.AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if (withDeleted)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return await queryable.AnyAsync(cancellationToken);
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default)
        {
            entity.DeletedOnUtc = DateTime.UtcNow;
            entity.IsDeleted = true;
            if (permanent)
            {
                _context.Remove(entity);
            }
            else
            {
                _context.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, CancellationToken cancellationToken = default)
        {
            foreach(TEntity entity in entities)
            {
                entity.DeletedOnUtc = DateTime.UtcNow;
                entity.IsDeleted = true;
            }

            if (permanent)
            {
                _context.RemoveRange(entities);
            }
            else
            {
                _context.UpdateRange(entities);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entities;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();
            if(!enableTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            if(include != null)
            {
                queryable = include(queryable); 
            }

            if (withDeleted)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();

            if (!enableTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            if(include != null)
            {
                queryable = include(queryable);
            }

            if (withDeleted)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            if (orderBy != null)
            {
                return await orderBy(queryable).ToPaginateAsync(index, size, 0, cancellationToken);
            }

            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public async Task<IPaginate<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);

            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            if (withDeleted)
                queryable = queryable.IgnoreQueryFilters();
            if (predicate != null)
                queryable = queryable.Where(predicate);
            return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.ModifiedOnUtc = DateTime.UtcNow;
            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach(TEntity entity in  entities)
                entity.ModifiedOnUtc = DateTime.UtcNow;

            _context.UpdateRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
            return entities;
        }
    }
}
