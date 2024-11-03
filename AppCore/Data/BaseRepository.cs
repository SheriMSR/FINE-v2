using System.Linq.Expressions;
using AppCore.Extensions;
using AppCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Data;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    public Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        int skip,
        int limit);

    public Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy);

    public Task<List<TEntity>> FindAsync(List<Guid> ids);

    public Task<List<TDto>> FindAsync<TDto>(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy);

    public Task<List<TDto>> FindAsync<TDto>(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        int skip,
        int limit);

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>>[] filters);

    public Task<bool> IsAlreadyExistAsync(
        Expression<Func<TEntity, bool>>[] filters);

    public Task<bool> IsAlreadyExistAsync(Guid systemId);
    public Task<bool> IsAlreadyExistAsync(IEnumerable<Guid> systemIds);

    public Task<FindResult<TEntity>> FindResultAsync(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        int skip,
        int limit);

    public Task<FindResult<TDto>> FindResultAsync<TDto>(
        Expression<Func<TEntity, bool>>[] filters,
        string orderBy,
        int skip,
        int limit);

    public Task<FindResult<TEntity>> FindResultAsync<TQuery>(
        Expression<Func<TEntity, bool>>[] filters, TQuery tQuery) where TQuery : BaseQuery;

    public Task<FindResult<TDto>> FindResultAsync<TDto, TQuery>(
        Expression<Func<TEntity, bool>>[] filters, TQuery tQuery) where TQuery : BaseQuery;

    public Task<TEntity> FindOneAsync(Guid systemId);
    public Task<TDto> FindOneAsync<TDto>(Guid systemId);

    public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>>[] filters,
        string orderBy = null);

    public Task<TDto> FindOneAsync<TDto>(Expression<Func<TEntity, bool>>[] filters,
        string orderBy = null);

    public Task<bool> InsertAsync(TEntity entity, Guid? creatorId, DateTime? now = null,
        bool isTesting = false);

    public Task<bool> InsertAsync(IEnumerable<TEntity> entities, Guid? creatorId,
        DateTime? now = null,
        bool isTesting = false);

    public Task<bool> UpdateAsync(TEntity entity, Guid? editorId, DateTime? now = null,
        bool isTesting = false);

    public Task<bool> UpdateAsync(IEnumerable<TEntity> entities, Guid? editorId,
        DateTime? now = null,
        bool isTesting = false);

    public Task<bool> DeleteAsync(TEntity entity, Guid? deleterId, DateTime? now = null,
        bool isTesting = false);

    public Task<bool> DeleteAsync(IEnumerable<TEntity> entities, Guid? deleterId,
        DateTime? now = null,
        bool isTesting = false);

    public IQueryable<TEntity> GetQuery();

    public IQueryable<TEntity> GetQueryAll();

    public IQueryable<TEntity> GetQueryNoTracking();
}

public class BaseRepository<TEntity>(DbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
    private readonly UnitOfWork _unitOfWork = new(dbContext);

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>[] filters, string orderBy, int skip,
        int limit)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        return await query.ToListAsync();
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _dbSet.Where(x => !x.IsDeleted);
    }

    public IQueryable<TEntity> GetQueryNoTracking()
    {
        return _dbSet.Where(x => !x.IsDeleted).AsNoTracking();
    }

    public IQueryable<TEntity> GetQueryAll()
    {
        return _dbSet;
    }

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>[] filters, string orderBy)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        return await query.ToListAsync();
    }

    public async Task<List<TEntity>> FindAsync(List<Guid> ids)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x =>
            !x.IsDeleted &&
            ids.Contains(x.Id)
        );
        return await query.ToListAsync();
    }


    public async Task<List<TDto>> FindAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        var result = await query.ToListAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<List<TDto>> FindAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy, int skip,
        int limit)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        var result = await query.ToListAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<List<TDto>> FindAsync<TDto>(List<Guid> ids)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x =>
            !x.IsDeleted &&
            ids.Contains(x.Id)
        );
        var result = await query.ToListAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>[] filters)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.CountAsync();
    }

    public async Task<bool> IsAlreadyExistAsync(Expression<Func<TEntity, bool>>[] filters)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.AnyAsync();
    }

    public async Task<bool> IsAlreadyExistAsync(Guid systemId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == systemId && !x.IsDeleted) != null;
    }

    public async Task<bool> IsAlreadyExistAsync(IEnumerable<Guid> systemIds)
    {
        return await _dbSet.CountAsync(x => !x.IsDeleted && systemIds.Contains(x.Id)) == systemIds.Count();
    }

    public async Task<FindResult<TEntity>> FindResultAsync(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        int skip, int limit)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        var totalCount = await query.LongCountAsync();
        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        var items = await query.ToListAsync();
        return FindResult<TEntity>.Success(items, totalCount);
    }

    public async Task<FindResult<TDto>> FindResultAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy,
        int skip, int limit)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        var totalCount = await query.LongCountAsync();
        if (skip > 0)
            query = query.Skip(skip);

        if (limit > 0)
            query = query.Take(limit);

        var result = await query.ToListAsync();
        return FindResult<TDto>.Success(result.ProjectTo<TEntity, TDto>(), totalCount);
    }

    public async Task<FindResult<TEntity>> FindResultAsync<TQuery>(Expression<Func<TEntity, bool>>[] filters,
        TQuery tQuery) where TQuery : BaseQuery
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        if (tQuery.CreatedAt != null && tQuery.CreatedAt != DateTime.MinValue)
        {
            query = query.Where(x =>
                x.CreatedAt.Date == tQuery.CreatedAt.Value.Date);
        }

        if (tQuery.CreateAtFrom != null && tQuery.CreateAtFrom != DateTime.MinValue)
        {
            query = query.Where(x =>
                x.CreatedAt >= tQuery.CreateAtFrom.Value);
        }

        if (tQuery.CreateAtTo != null && tQuery.CreateAtTo != DateTime.MinValue)
        {
            query = query.Where(x =>
                x.CreatedAt >= tQuery.CreateAtTo.Value);
        }

        if (!string.IsNullOrEmpty(tQuery.OrderBy))
            query = OrderBy(query, tQuery.OrderBy);

        var totalCount = await query.LongCountAsync();
        if (tQuery.Skip() > 0)
            query = query.Skip(tQuery.Skip());

        if (tQuery.PageSize > 0)
            query = query.Take(tQuery.PageSize);

        var items = await query.ToListAsync();
        return FindResult<TEntity>.Success(items, totalCount);
    }

    public async Task<FindResult<TDto>> FindResultAsync<TDto, TQuery>(Expression<Func<TEntity, bool>>[] filters,
        TQuery tQuery) where TQuery : BaseQuery
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        if (tQuery.CreatedAt != null && tQuery.CreatedAt != DateTime.MinValue)
        {
            query = query.Where(x =>
                x.CreatedAt.Date == tQuery.CreatedAt.Value.Date);
        }

        if (tQuery.CreateAtFrom != null && tQuery.CreateAtFrom != DateTime.MinValue)
        {
            query = query.Where(x =>
                x.CreatedAt >= tQuery.CreateAtFrom.Value);
        }

        if (tQuery.CreateAtTo != null && tQuery.CreateAtTo != DateTime.MinValue)
        {
            query = query.Where(x =>
                x.CreatedAt >= tQuery.CreateAtTo.Value);
        }

        if (!string.IsNullOrEmpty(tQuery.OrderBy))
            query = OrderBy(query, tQuery.OrderBy);

        var totalCount = await query.LongCountAsync();
        if (tQuery.Skip() > 0)
            query = query.Skip(tQuery.Skip());

        if (tQuery.PageSize > 0)
            query = query.Take(tQuery.PageSize);

        var items = await query.ToListAsync();
        return FindResult<TDto>.Success(items.ProjectTo<TEntity, TDto>(), totalCount);
    }

    public async Task<TEntity> FindOneAsync(Guid systemId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == systemId && !x.IsDeleted);
    }

    public async Task<TDto> FindOneAsync<TDto>(Guid systemId)
    {
        var a = await _dbSet.FirstOrDefaultAsync(x => x.Id == systemId && !x.IsDeleted);
        return a.ProjectTo<TEntity, TDto>();
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>>[] filters, string orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<TDto> FindOneAsync<TDto>(Expression<Func<TEntity, bool>>[] filters, string orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet;
        query = query.Where(x => !x.IsDeleted);
        if (filters != null && filters.Any())
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));

        if (!string.IsNullOrEmpty(orderBy))
            query = OrderBy(query, orderBy);

        var result = await query.FirstOrDefaultAsync();
        return result.ProjectTo<TEntity, TDto>();
    }

    public async Task<bool> InsertAsync(TEntity entity, Guid? creatorId, DateTime? now = null,
        bool isTesting = false)
    {
        await _unitOfWork.BeginTransactionAsync();
        now ??= DateTime.Now;
        entity.CreatedAt = now.Value;
        entity.EditedAt = now.Value;
        entity.CreatorId = creatorId;
        entity.EditorId = creatorId;
        entity.IsTesting = isTesting;
        await _dbSet.AddAsync(entity);
        await _unitOfWork.SaveAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }

    public async Task<bool> InsertAsync(IEnumerable<TEntity> entities, Guid? creatorId, DateTime? now = null,
        bool isTesting = false)
    {
        await _unitOfWork.BeginTransactionAsync();
        now ??= DateTime.Now;
        entities = entities.Select(x =>
        {
            x.CreatedAt = now.Value;
            x.EditedAt = now.Value;
            x.CreatorId = creatorId;
            x.EditorId = creatorId;
            x.IsTesting = isTesting;
            return x;
        });
        await _dbSet.AddRangeAsync(entities);
        await _unitOfWork.SaveAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }

    public async Task<bool> UpdateAsync(TEntity entity, Guid? editorId, DateTime? now = null, bool isTesting = false)
    {
        await _unitOfWork.BeginTransactionAsync();
        now ??= DateTime.Now;
        entity.EditedAt = now.Value;
        entity.EditorId = editorId;
        entity.IsTesting = isTesting;
        _dbSet.Entry(entity).State = EntityState.Modified;
        await _unitOfWork.SaveAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }

    public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities, Guid? editorId, DateTime? now = null,
        bool isTesting = false)
    {
        await _unitOfWork.BeginTransactionAsync();
        now ??= DateTime.Now;
        entities = entities.Select(x =>
        {
            x.EditedAt = now.Value;
            x.EditorId = editorId;
            x.IsTesting = isTesting;
            return x;
        });
        foreach (var entity in entities) _dbSet.Entry(entity).State = EntityState.Modified;

        await _unitOfWork.SaveAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }

    public async Task<bool> DeleteAsync(TEntity entity, Guid? deleterId, DateTime? now = null, bool isTesting = false)
    {
        await _unitOfWork.BeginTransactionAsync();
        now ??= DateTime.Now;
        entity.DeletedAt = now.Value;
        entity.DeleterId = deleterId;
        entity.IsDeleted = true;
        entity.IsTesting = isTesting;
        _dbSet.Entry(entity).State = EntityState.Modified;
        await _unitOfWork.SaveAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }

    public async Task<bool> DeleteAsync(IEnumerable<TEntity> entities, Guid? deleterId, DateTime? now = null,
        bool isTesting = false)
    {
        await _unitOfWork.BeginTransactionAsync();
        now ??= DateTime.Now;
        entities = entities.Select(x =>
        {
            x.DeletedAt = now.Value;
            x.DeleterId = deleterId;
            x.IsDeleted = true;
            x.IsTesting = isTesting;
            return x;
        });
        foreach (var entity in entities) _dbSet.Entry(entity).State = EntityState.Modified;

        await _unitOfWork.SaveAsync();
        return await _unitOfWork.CommitTransactionAsync();
    }

    private static IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, string orderBy)
    {
        orderBy = orderBy.ToCamel();
        var propertyName = orderBy.Split(" ")[0];
        query = orderBy.Contains("desc")
            ? query.OrderByDescending(x => EF.Property<TEntity>(x, propertyName))
            : query.OrderBy(x => EF.Property<TEntity>(x, propertyName));
        return query;
    }
}