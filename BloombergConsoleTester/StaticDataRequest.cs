using System;
using BenchmarkPlus.CommonDomain.Extensions;
using BloombergConsoleTester.Infrastructure.NHibernate;
using BloombergConsoleTester.Infrastructure.Repositories;

namespace BloombergConsoleTester
{
    public static class StaticDataRequest
    {
        public static void RunExample(string security)
        {
            var bloomberg = new Bloomberg(new BloombergSessionFactory());

            var repo = new IndexRepository(new SessionBuilder(new SessionFactoryBuilder(new ConfigurationFactory())));
            var indexes = repo.GetById(1, 1481);
            var totalReturns = bloomberg.GetTotalReturnsFor(indexes);

            foreach (var totalReturn in totalReturns)
            {
                Console.WriteLine(@"Index:           {0}", totalReturn.Security.Name);
                Console.WriteLine(@"Ticker:          {0}", totalReturn.Security.BloombergTicker);
                Console.WriteLine(@"Use Underlying?: {0}", totalReturn.Security.UseUnderlyingPriceForReturn ? @"True" : "False");
                Console.WriteLine(@"");
                Console.WriteLine("{0}\t{1}\t\t{2}", @"DateTime", @"Value", @"Return");
                Console.WriteLine("{0}\t{1}\t\t{2}", @"--------", @"-----", @"------");
                foreach (var @return in totalReturn)
                {
                    Console.WriteLine("{0}\t{1}\t\t{2}",
                                      @return.DateTime.ToString(@"MM-dd-yyyy"),
                                      @return.Value.IfNotNull(x => x.ToString("0.00")),
                                      @return.Return.IfNotNull(x => x.ToString("0.00")));
                }
                Console.WriteLine(@"");
            }
        }
    }
}