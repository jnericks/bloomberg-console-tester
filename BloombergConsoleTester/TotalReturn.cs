using System;
using BenchmarkPlus.CommonDomain.ValueObjects;

namespace BloombergConsoleTester
{
    public class TotalReturn
    {
        internal TotalReturn _previous;
        internal TotalReturn _next;

        public DateTime DateTime { get; set; }
        public decimal? Value { get; set; }
        public Percent Return { get; set; }

        public TotalReturn(DateTime dateTime, decimal? value = 0)
        {
            DateTime = dateTime;
            Value = value;
        }

        public decimal? GetLastValue()
        {
            var prev = _previous;

            if (prev == null) return null;

            return prev.Value != null
                ? prev.Value.Value
                : prev.GetLastValue();
        }
    }
}