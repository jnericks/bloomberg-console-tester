using System.Collections;
using System.Collections.Generic;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public class InstanceScoper<T> : InstanceScoperBase<T>
    {
        private static readonly IDictionary _dictionary = new Dictionary<object, T>();

        protected override IDictionary GetDictionary()
        {
            return _dictionary;
        }
    }
}