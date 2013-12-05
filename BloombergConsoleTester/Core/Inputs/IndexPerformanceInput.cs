using BenchmarkPlus.CommonDomain.ValueObjects;

namespace BloombergConsoleTester.Core.Inputs
{
    public class IndexPerformanceInput
    {
        public YearMonth YearMonth { get; protected set; }
        public decimal? Return { get; protected set; }
        public decimal? Value { get; protected set; }

        public IndexPerformanceInput(YearMonth performanceDate, decimal? @return, decimal? value)
        {
            YearMonth = performanceDate;
            Return = @return;
            Value = value;
        }
    }
}