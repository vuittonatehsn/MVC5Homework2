using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MVC5Homework2.Models
{
	public class EFRepository<T> : IRepository<T> where T : class
	{
		public IUnitOfWork UnitOfWork { get; set; }
		
		private IDbSet<T> _objectset;
		private IDbSet<T> ObjectSet
		{
			get
			{
				if (_objectset == null)
				{
					_objectset = UnitOfWork.Context.Set<T>();
				}
				return _objectset;
			}
		}

		public virtual IQueryable<T> All()
		{
			return ObjectSet.AsQueryable();
		}

		public IQueryable<T> Where(Expression<Func<T, bool>> expression)
		{
			return ObjectSet.Where(expression);
		}

		public virtual void Add(T entity)
		{
			ObjectSet.Add(entity);
		}

		public virtual void Delete(T entity)
		{
			ObjectSet.Remove(entity);
		}

        public IQueryable<T> FindBy(Func<T, bool> predicate, string lazyIncludeString)
        {
            return ObjectSet.Include(lazyIncludeString).Where(predicate).AsQueryable<T>();
        }

        public virtual IQueryable<T> GetSort(Expression<Func<T, bool>> filter = null
            , Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            , string includeProperties = "")
        {
            IQueryable<T> query = ObjectSet;

            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            query = properties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query) : query;
        }


    }
}