using BloombergConsoleTester.Core.Models;
using FluentNHibernate;

namespace BloombergConsoleTester.Infrastructure.Mappings
{
    public class IndexMap : PersistentObjectClassMap<Core.Models.Index>
    {
        public override string TableName
        {
            get { return "vwIndexUpdaterIndex"; }
        }

        public override string KeyColumn
        {
            get { return "IndexKey"; }
        }

        public IndexMap()
        {
            Map(x => x.Name).Column("IndexName");
            Map(x => x.BloombergTicker).Column("IndexTicker_Bloomberg");
            Map(x => x.BloombergDatabase).Column("IndexBloombergDatabase");
            Map(x => x.CurrencyCode).Column("IndexCurrencyCode");
            Map(x => x.FutureUnderlyingTicker).Column("IndexFuture_UnderlyingTicker");
            Map(x => x.FutureRiskFreeTicker).Column("IndexFuture_RiskFreeTicker");
            Map(x => x.UnderlyingCurrencyCode).Column("IndexFuture_UnderlyingCurrencyCode");
            Map(x => x.MinPeriodsBack).Column("IndexMinPeriodsBack");
            Map(x => x.MaxPeriodsBack).Column("IndexMaxPeriodsBack");
            Component(x => x.EarliestMonthYearToUpdate, ComponentPart.MonthYearForColumn("`EarliestDateToUpdate`"));
            Map(x => x.UseUnderlyingPriceForReturn).Column("IndexUseUnderlyingPriceForReturn");
            Map(x => x.IsUpdatedByHand);
            Component(x => x.LastPerformanceMonthYear, ComponentPart.MonthYearForColumn("`LastPerformanceDate`")).ReadOnly();
            Map(x => x.LastPerformanceDay).ReadOnly();
            Map(Reveal.Member<Core.Models.Index>("_indexHedgeableFlag")).Column("IndexHedgeableFlag");
            Map(Reveal.Member<Core.Models.Index>("_indexStoreValueFlag")).Column("IndexStoreValueFlag");

            HasMany<IndexPerformance>(Reveal.Member<Core.Models.Index>("_indexPerformances"))
                .Fetch.Subselect()
                .Cascade.AllDeleteOrphan();

            HasMany<IndexDailyPerformance>(Reveal.Member<Core.Models.Index>("_indexDailyPerformances"))
                .Fetch.Subselect()
                .Cascade.AllDeleteOrphan();
        }
    }
}