using BenchmarkPlus.CommonDomain;
using BenchmarkPlus.CommonDomain.ValueObjects;

namespace BloombergConsoleTester.Core.Models
{
    public class IndexPerformance : PersistentObject
    {
        int _month;
        int _year;

        public virtual Index Index { get; protected set; }
        public virtual YearMonth YearMonth { get; protected set; }
        public virtual decimal? Return { get; protected set; }
        public virtual decimal? Value { get; protected set; }

        protected IndexPerformance() { }

        public IndexPerformance(Index index, YearMonth yearMonth)
            : this(index, yearMonth, null, null) { }

        public IndexPerformance(Index index, YearMonth yearMonth, decimal? @return, decimal? value)
        {
            Index = index;
            YearMonth = yearMonth;
            _month = yearMonth.Month;
            _year = yearMonth.Year;
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