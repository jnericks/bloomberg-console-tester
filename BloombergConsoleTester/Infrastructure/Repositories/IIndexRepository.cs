using System.Collections.Generic;
using BloombergConsoleTester.Core.Models;

namespace BloombergConsoleTester.Infrastructure.Repositories
{
    public interface IIndexRepository
    {
        Index GetById(int indexKey);
        IEnumerable<Index> GetById(params int[] indexKeys);

        void Save(Index index);
    }
}