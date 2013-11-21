using System;
using BenchmarkPlus.CommonDomain.ValueObjects;

namespace BloombergConsoleTester
{
    public class BBIndexReturn
    {
        internal BBIndexReturn _previous;
        internal BBIndexReturn _next;

        public DateTime DateTime { get; set; }
        public decimal? Value { get; set; }
        public Percent Return { get; set; }

        public BBIndexReturn(DateTime dateTime, decimal? value = 0)
        {
            DateTime = dateTime;
            Value = value;
        }
    }
}