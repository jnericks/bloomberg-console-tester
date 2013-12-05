using BloombergConsoleTester.Core.Models;

namespace BloombergConsoleTester.Infrastructure.Mappings
{
    public class IndexDailyPerformanceMap : PersistentObjectClassMap<IndexDailyPerformance>
    {
        public IndexDailyPerformanceMap()
        {
            References(x => x.Index);
            Map(x => x.Date).Column(@"`Date`");
            Map(x => x.Return).Column(@"`Return`");
            Map(x => x.Value).Column(@"`Value`");
        }
    }
}