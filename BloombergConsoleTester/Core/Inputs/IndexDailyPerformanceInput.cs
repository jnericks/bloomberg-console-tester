using System;

namespace BloombergConsoleTester.Core.Inputs
{
    public class IndexDailyPerformanceInput
    {
        public DateTime Date { get; protected set; }
        public decimal? Return { get; protected set; }
        public decimal? Value { get; protected set; }

        public IndexDailyPerformanceInput(DateTime performanceDate, decimal? @return, decimal? value)
        {
            Date = performanceDate;
            Return = @return;
            Value = value;
        }
    }
}