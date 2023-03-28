using EventCommunity.Core.Entities;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Interfaces;
using EventCommunity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventCommunity.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DatabaseContext context;

        protected DbSet<TEntity> dbSet;

        public GenericRepository(DatabaseContext context)
        {
            this.context = context;

            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query.AsNoTracking());
            }
            else
            {
                return query.AsNoTracking();
            }
        }

        public TEntity? GetById(int id)
        {
            return dbSet.AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public virtual async Task<int> Create(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = GetById(id);

            if (entity == null)
            {
                throw new EntityDoesNotExistException(typeof(TEntity), id);
            }

            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
