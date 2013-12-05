using System;
using System.Data.SqlClient;
using BenchmarkPlus.CommonDomain.Extensions;
using BloombergConsoleTester.Infrastructure.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Cfg;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public static Func<string> ConnectionString = () => { throw new ApplicationException("Need to Configure BenchmarkPlus.RMS.Infrastructure.NHibernate.ConfigurationFactory.ConnectionString()"); };

        public static string ConnectedTo;

        public static IConvention[] GetConventions()
        {
            return new IConvention[]
            {
                PrimaryKey.Name.Is(p => "`Key`"),
                ForeignKey.EndsWith("Key"),
                DefaultAccess.Property(),
                DefaultCascade.All(),
                DefaultLazy.Always(),
                AutoImport.Never(),
            };
        }

        public Configuration Build()
        {
            var conventions = GetConventions();

            using (var conn = new SqlConnection(ConnectionString()))
            {
                ConnectedTo = @"{0}\{1}".FormatWith(conn.DataSource, conn.Database);

                var rawConnectionString = conn.ConnectionString;

                return Fluently.Configure()
                               .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(rawConnectionString)).AdoNetBatchSize(200))
                               .Mappings(m => m.FluentMappings.AddFromAssemblyOf<IndexMap>().Conventions.Add(conventions))
                               .BuildConfiguration();
            }
        }
    }
}