using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework
{
    public interface IRepository<T> where T: IAggregate
    {
        Task<T> GetById(Guid aggregateId);
        Task<List<T>> GetAll();
    }
}