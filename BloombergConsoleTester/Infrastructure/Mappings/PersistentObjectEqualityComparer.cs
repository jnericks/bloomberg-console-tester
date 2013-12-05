using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkPlus.CommonDomain;

namespace BloombergConsoleTester.Infrastructure.Mappings
{
    public class PersistentObjectEqualityComparer : IEqualityComparer, IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            if (x is IEnumerable && y is IEnumerable)
            {
                return ((IEnumerable)x).OfType<object>().SequenceEqual(((IEnumerable)y).OfType<object>(), this);
            }
            if (x is PersistentObject && y is PersistentObject)
            {
                return ((PersistentObject)x).Id == ((PersistentObject)y).Id;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}