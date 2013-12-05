using System;
using BenchmarkPlus.CommonDomain;

namespace BloombergConsoleTester.Core.Models
{
    public class IndexDailyPerformance : PersistentObject
    {
        public virtual Index Index { get; protected set; }
        public virtual DateTime Date { get; protected set; }
        public virtual decimal? Return { get; protected set; }
        public virtual decimal? Value { get; protected set; }

        protected IndexDailyPerformance() { }

        public IndexDailyPerformance(Index index, DateTime dateTime)
            : this(index, dateTime, null, null) { }

        public IndexDailyPerformance(Index index, DateTime dateTime, decimal? @return, decimal? value)
        {
            Index = index;
            Date = dateTime;
            Return = @return;
            Value = value;
        }

        public virtual void SetPerformanceReturn(decimal? @return)
        {
            Return = @return;
        }

        public virtual void SetPerformanceValue(decimal? value)
        {
            Value = value;
        }
    }
}