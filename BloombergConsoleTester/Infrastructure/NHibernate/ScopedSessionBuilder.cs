using NHibernate;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public class ScopedSessionBuilder : IScopedSessionBuilder
    {
        IScope _scope;
        ISessionFactoryBuilder _builder;
        IInstanceScoper<ISession> _sessionInstanceScoper;

        public ScopedSessionBuilder(ISessionFactoryBuilder builder, IScope scope)
        {
            _scope = scope;
            _builder = builder;
            _sessionInstanceScoper = new InstanceScoper<ISession>();
        }

        public ISession GetSession()
        {
            var session = GetScopedInstance();
            if (session.IsOpen) return session;

            _sessionInstanceScoper.ClearScopedInstance(_scope);
            return GetScopedInstance();
        }

        public void ClearSession()
        {
            _sessionInstanceScoper.ClearScopedInstance(_scope);
        }

        ISession BuildSession()
        {
            var factory = _builder.GetFactory();
            var session = factory.OpenSession();
            session.FlushMode = FlushMode.Commit;
            return session;
        }

        ISession GetScopedInstance()
        {
            return _sessionInstanceScoper.GetScopedInstance(_scope, BuildSession);
        }

        public void Dispose()
        {
            GetSession().Dispose();
            ClearSession();
        }
    }
}