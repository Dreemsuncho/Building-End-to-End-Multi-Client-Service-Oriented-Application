using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Contracts;
using Core.Common.Utils;

namespace Core.Common.Data
{
    public abstract class DataRepositoryBase<T, U> : IDatRepository<T>
        where T : class, IIdentifiableEntity, new()
        where U : DbContext, new()
    {
        protected abstract T AddEntity(U entityContext, T entity);

        protected abstract T UpdateEntity(U entityContext, T entity);

        protected abstract IEnumerable<T> GetEntities(U entityContext);

        protected abstract T GetEntity(U entityContext, int id);

        public T Add(T entity)
        {
            using (U context = new U())
            {
                T addedEntity = AddEntity(context, entity);
                context.SaveChanges();
                return addedEntity;
            }
        }

        public IEnumerable<T> Get()
        {
            using (U context = new U())
            {
                return this.GetEntities(context);
            }
        }

        public T Get(int id)
        {
            using (U context = new U())
            {
                return this.GetEntity(context, id);
            }
        }

        public void Remove(int id)
        {
            using (U context = new U())
            {
                T entity = this.GetEntity(context, id);
                context.Entry<T>(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Remove(T entity)
        {
            using (U context = new U())
            {
                context.Entry<T>(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (U context = new U())
            {
                T existingEntity = this.UpdateEntity(context, entity);
                SimpleMapper.PropertyMap(entity, existingEntity);

                context.SaveChanges();
                return existingEntity;
            }
        }
    }
}
