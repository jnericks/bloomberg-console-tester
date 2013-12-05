namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public interface IScopedSessionBuilder : ISessionBuilder
    {
        void ClearSession();
    }
}