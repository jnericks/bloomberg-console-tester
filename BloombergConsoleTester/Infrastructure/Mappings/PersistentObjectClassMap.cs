using System;
using System.Linq;
using BenchmarkPlus.CommonDomain;
using BenchmarkPlus.CommonDomain.Extensions;
using FluentNHibernate.Mapping;

namespace BloombergConsoleTester.Infrastructure.Mappings
{
    public abstract class PersistentObjectClassMap<T> : ClassMap<T> where T : PersistentObject
    {
        public virtual string TableName
        {
            get { return GetType().BaseType.GetGenericArguments().First().Name; }
        }

        public virtual string KeyColumn
        {
            get { return @"Key"; }
        }

        public virtual Func<IdentityGenerationStrategyBuilder<IdentityPart>, IdentityPart> POIDGenerator
        {
            get { return x => x.Identity(); }
        }

        protected virtual Func<IdentityGenerationStrategyBuilder<IdentityPart>, IdentityPart> HiLo
        {
            get { return x => x.HiLo("NHibHiLoGenerator", "MaxLo", "1000", "TableName='{0}'".FormatWith(TableName)); }
        }

        protected PersistentObjectClassMap()
        {
            Table(NHibFormat(TableName));
            POIDGenerator(Id(x => x.Id).Column(NHibFormat(KeyColumn)).GeneratedBy);
        }

        protected string NHibFormat(string item)
        {
            return @"`{0}`".FormatWith(item);
        }
    }
}