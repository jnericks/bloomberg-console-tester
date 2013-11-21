using System;

namespace BloombergConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = string.Empty;

            //Console.WriteLine(@"Type security (eg. SPX INDEX)...");
            //Console.WriteLine();
            //var input = Console.ReadLine();
            Console.WriteLine(@"Push enter to start the application.");
            Console.WriteLine(string.Empty);
            Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) input = @"SPX INDEX";

            HistoricalDataRequest.RunExample(input);

            Console.WriteLine(string.Empty);
            Console.WriteLine(@"Push enter to quit the application.");
            Console.ReadLine();
        }
    }
}
