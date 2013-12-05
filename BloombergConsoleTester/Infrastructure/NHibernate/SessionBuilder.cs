using NHibernate;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public class SessionBuilder : ISessionBuilder
    {
        const string NHIBERNATE_SESSION = "NHibernate.ISession.F0464D34-60DD-47BD-87BD-802570A75347";
        ISessionFactoryBuilder _builder;
        IInstanceScoper<ISession> _sessionInstanceScoper;

        public SessionBuilder(ISessionFactoryBuilder builder)
        {
            _builder = builder;
            _sessionInstanceScoper = new InstanceScoper<ISession>();
        }

        public ISession GetSession()
        {
            var session = GetScopedInstance();
            if (session.IsOpen) return session;

            _sessionInstanceScoper.ClearScopedInstance(NHIBERNATE_SESSION);
            return GetScopedInstance();
        }

        private ISession BuildSession()
        {
            var factory = _builder.GetFactory();
            var session = factory.OpenSession();
            session.FlushMode = FlushMode.Commit;
            return session;
        }

        private ISession GetScopedInstance()
        {
            return _sessionInstanceScoper.GetScopedInstance(NHIBERNATE_SESSION, BuildSession);
        }

        public void Dispose()
        {
            GetSession().Dispose();
            _sessionInstanceScoper.ClearScopedInstance(NHIBERNATE_SESSION);
        }
    }
}