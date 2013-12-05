using System;
using BloombergConsoleTester.Infrastructure.NHibernate;

namespace BloombergConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationFactory.ConnectionString = () => @"Data Source=MOTHERSHIP;Initial Catalog=bpUserAcceptanceTester;Persist Security Info=True;User ID=bpUser;Password=1paradigm2;";
            var input = string.Empty;

            //Console.WriteLine(@"Name Index (eg. SPX INDEX)...");
            //Console.WriteLine();
            //var input = Console.ReadLine();
            Console.WriteLine(@"Push enter to start the application.");
            Console.WriteLine(string.Empty);
            Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) input = @"SPX INDEX";

            StaticDataRequest.RunExample(input);

            Console.WriteLine(string.Empty);
            Console.WriteLine(@"Push enter to quit the application.");
            Console.ReadLine();
        }
    }
}
