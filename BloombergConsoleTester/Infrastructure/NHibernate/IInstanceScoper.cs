using System;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public interface IInstanceScoper<T>
    {
        void ClearScopedInstance(object key);
        T GetScopedInstance(object key, Func<T> builder);
    }
}