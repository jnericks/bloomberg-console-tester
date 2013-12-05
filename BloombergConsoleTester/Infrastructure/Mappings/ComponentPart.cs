using System;
using BenchmarkPlus.CommonDomain.ValueObjects;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace BloombergConsoleTester.Infrastructure.Mappings
{
    public static class ComponentPart
    {
        public static Action<ComponentPart<YearMonth>> MonthYearForColumn(string columnName)
        {
            return x => x.Map(Reveal.Member<YearMonth>("_dateTime"))
                         .Access.Field()
                         .Column(columnName);
        }
    }
}