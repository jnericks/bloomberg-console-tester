using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkPlus.CommonDomain;
using BenchmarkPlus.CommonDomain.Extensions;
using BenchmarkPlus.CommonDomain.ValueObjects;
using BloombergConsoleTester.Core.Inputs;

namespace BloombergConsoleTester.Core.Models
{
    public class Index : PersistentObject
    {
        int _indexHedgeableFlag;
        int _indexStoreValueFlag;
        IList<IndexPerformance> _indexPerformances;
        IList<IndexDailyPerformance> _indexDailyPerformances;

        public virtual string Name { get; protected set; }
        public virtual string BloombergTicker { get; protected set; }
        public virtual string BloombergDatabase { get; protected set; }
        public virtual string CurrencyCode { get; protected set; }
        public virtual string UnderlyingCurrencyCode { get; protected set; }
        public virtual string FutureUnderlyingTicker { get; protected set; }
        public virtual string FutureRiskFreeTicker { get; protected set; }
        public virtual int MinPeriodsBack { get; protected set; }
        public virtual int? MaxPeriodsBack { get; protected set; }
        public virtual YearMonth EarliestMonthYearToUpdate { get; set; }
        public virtual bool UseUnderlyingPriceForReturn { get; protected set; }
        public virtual bool IsUpdatedByHand { get; protected set; }

        public virtual string BloombergName
        {
            get { return @"{0} {1}".FormatWith(BloombergTicker, BloombergDatabase); }
        }

        public virtual bool ShouldStoreValue
        {
            get { return _indexStoreValueFlag != 0; }
            set { _indexStoreValueFlag = value ? 1 : 0; }
        }

        public virtual bool IsHedgeable
        {
            get { return _indexHedgeableFlag != 0; }
            protected set { _indexHedgeableFlag = value ? 1 : 0; }
        }

        public virtual YearMonth LastPerformanceMonthYear { get; protected set; }
        public virtual DateTime? LastPerformanceDay { get; set; }

        public virtual IEnumerable<IndexPerformance> IndexPerformances
        {
            get { return _indexPerformances.ToEnumerable(); }
        }

        public virtual IEnumerable<IndexDailyPerformance> IndexDailyPerformances
        {
            get { return _indexDailyPerformances.ToEnumerable(); }
        }

        protected Index()
            : this(string.Empty) { }

        public Index(string name)
        {
            Name = name;
            _indexPerformances = new List<IndexPerformance>();
            _indexDailyPerformances = new List<IndexDailyPerformance>();
        }

        public Index(string name,
                     string bloombergTicker,
                     string bloombergDatabase,
                     string currencyCode,
                     string underlyingCurrencyCode,
                     string futureUnderlyingTicker,
                     string futureRiskFreeTicker,
                     int minPeriodsBack,
                     int maxPeriodsBack,
                     bool useUnderlyingPriceForReturn,
                     bool isUpdatedByHand,
                     bool shouldStoreValue)
            : this(name)
        {
            BloombergTicker = bloombergTicker;
            BloombergDatabase = bloombergDatabase;
            CurrencyCode = currencyCode;
            FutureUnderlyingTicker = futureUnderlyingTicker;
            FutureRiskFreeTicker = futureRiskFreeTicker;
            UnderlyingCurrencyCode = underlyingCurrencyCode;
            MinPeriodsBack = minPeriodsBack;
            MaxPeriodsBack = maxPeriodsBack;
            UseUnderlyingPriceForReturn = useUnderlyingPriceForReturn;
            IsUpdatedByHand = isUpdatedByHand;
            _indexStoreValueFlag = shouldStoreValue ? 1 : 0;
        }

        public virtual void AddIndexPerformance(params IndexPerformanceInput[] inputs)
        {
            foreach (var input in inputs)
                _indexPerformances.Add(new IndexPerformance(this, input.YearMonth, input.Return, input.Value));
        }

        public virtual void AddIndexDailyPerformance(params IndexDailyPerformanceInput[] inputs)
        {
            foreach (var input in inputs)
                _indexDailyPerformances.Add(new IndexDailyPerformance(this, input.Date, input.Return, input.Value));
        }

        public virtual void AddOrUpdateIndexPerformances(IEnumerable<IndexPerformanceInput> inputs)
        {
            foreach (var input in inputs)
                AddOrUpdateIndexPerformance(input);
        }

        public virtual void AddOrUpdateIndexDailyPerformances(IEnumerable<IndexDailyPerformanceInput> inputs)
        {
            foreach (var input in inputs)
                AddOrUpdateIndexDailyPerformance(input);
        }

        public virtual void AddOrUpdateIndexPerformance(IndexPerformanceInput input)
        {
            if (EarliestMonthYearToUpdate != null && input.YearMonth < EarliestMonthYearToUpdate)
            {
                Log.Info(@"    Earliest Month Year is {0}, not adding performance data".FormatWith(EarliestMonthYearToUpdate.ToString()));
                return;
            }

            var ip = _indexPerformances.SingleOrDefault(x => x.YearMonth == input.YearMonth);

            var addedOrUpdated = "Updated";
            if (ip == null)
            {
                ip = new IndexPerformance(this, input.YearMonth);
                _indexPerformances.Add(ip);
                addedOrUpdated = "Added";
            }

            ip.SetPerformanceReturn(input.Return);
            Log.Info(@"    Return {0}: {1} = {2}".FormatWith(addedOrUpdated, input.YearMonth.ToString(), input.Return));

            if (!ShouldStoreValue) return;

            ip.SetPerformanceValue(input.Value);
            Log.Info(@"    Value {0}: {1} = {2}".FormatWith(addedOrUpdated, input.YearMonth.ToString(), input.Value));
        }

        public virtual void AddOrUpdateIndexDailyPerformance(IndexDailyPerformanceInput input)
        {
            if (EarliestMonthYearToUpdate != null && input.Date < EarliestMonthYearToUpdate.AsDateTime())
            {
                Log.Info(@"    Earliest Date is {0}, not adding performance data".FormatWith(EarliestMonthYearToUpdate.AsDateTime().ToShortDateString()));
                return;
            }

            var ip = _indexDailyPerformances.SingleOrDefault(x => x.Date == input.Date);

            var addedOrUpdated = "Updated";
            if (ip == null)
            {
                ip = new IndexDailyPerformance(this, input.Date);
                _indexDailyPerformances.Add(ip);
                addedOrUpdated = "Added";
            }

            ip.SetPerformanceReturn(input.Return);
            Log.Info(@"    Return {0}: {1} = {2}".FormatWith(addedOrUpdated, input.Date.ToShortDateString(), input.Return));

            if (!ShouldStoreValue) return;

            ip.SetPerformanceValue(input.Value);
            Log.Info(@"    Value {0}: {1} = {2}".FormatWith(addedOrUpdated, input.Date.ToShortDateString(), input.Value));
        }
    }
}