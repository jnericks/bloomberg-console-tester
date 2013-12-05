using NHibernate.Cfg;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public interface IConfigurationFactory
    {
        Configuration Build();
    }
}