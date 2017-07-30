using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IDataRepository
    {
    }

    public interface IDatRepository<T> : IDataRepository
        where T : class, IIdentifiableEntity, new()
    {
        T Add(T entity);

        void Remove(int id);

        void Remove(T entity);

        T Update(T entity);

        IEnumerable<T> Get();

        T Get(int id);
    }
}
