using BloombergConsoleTester.Core.Models;
using FluentNHibernate;

namespace BloombergConsoleTester.Infrastructure.Mappings
{
    public class IndexPerformanceMap : PersistentObjectClassMap<IndexPerformance>
    {
        public override string TableName
        {
            get { return "tblIndexPerformance"; }
        }

        public override string KeyColumn
        {
            get { return "IndexPerformanceKey"; }
        }

        public IndexPerformanceMap()
        {
            References(x => x.Index);
            Component(x => x.YearMonth, ComponentPart.MonthYearForColumn("`Date`"));
            Map(Reveal.Member<IndexPerformance>("_month")).Column("MonthKey");
            Map(Reveal.Member<IndexPerformance>("_year")).Column("YearKey");
            Map(x => x.Return).Column("IndexPerformanceReturn");
            Map(x => x.Value).Column("IndexPerformanceValue");
        }
    }
}