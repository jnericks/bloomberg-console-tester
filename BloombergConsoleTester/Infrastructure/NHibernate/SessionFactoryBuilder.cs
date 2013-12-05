using NHibernate;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public class SessionFactoryBuilder : ISessionFactoryBuilder
    {
        IConfigurationFactory _configurationFactory;
        ISessionFactory _factory;

        public SessionFactoryBuilder(IConfigurationFactory configurationFactory)
        {
            _configurationFactory = configurationFactory;
            _factory = BuildFactory();
        }

        public ISessionFactory GetFactory()
        {
            return _factory;
        }

        ISessionFactory BuildFactory()
        {
            var cfg = _configurationFactory.Build();
            var sessionFactory = cfg.BuildSessionFactory();
            return sessionFactory;
        }
    }
}