using NHibernate;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory GetFactory();
    }
}