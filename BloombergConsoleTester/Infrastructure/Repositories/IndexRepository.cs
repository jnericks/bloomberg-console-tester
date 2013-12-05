using System.Collections.Generic;
using System.Linq;
using BloombergConsoleTester.Core.Models;
using BloombergConsoleTester.Infrastructure.NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace BloombergConsoleTester.Infrastructure.Repositories
{
    public class IndexRepository : IIndexRepository
    {
        ISessionBuilder _sessionBuilder;

        public IndexRepository(ISessionBuilder sessionBuilder)
        {
            _sessionBuilder = sessionBuilder;
        }

        public Index GetById(int indexKey)
        {
            return GetSession().Get<Index>(indexKey);
        }

        public IEnumerable<Index> GetById(params int[] indexKeys)
        {
            if (indexKeys == null || indexKeys.Length <= 0) 
                return Enumerable.Empty<Index>();

            return GetSession().CreateCriteria<Index>().Add(Restrictions.In(@"Id", indexKeys)).List<Index>();
        }

        public void Save(Index index)
        {
            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.SaveOrUpdate(index);
                tx.Commit();
            }
        }

        ISession GetSession()
        {
            return _sessionBuilder.GetSession();
        }
    }
}