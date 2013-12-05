using System;
using NHibernate;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public interface ISessionBuilder : IDisposable
    {
        ISession GetSession();
    }
}