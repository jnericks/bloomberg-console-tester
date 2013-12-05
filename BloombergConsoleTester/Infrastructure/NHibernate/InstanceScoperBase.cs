using System;
using System.Collections;

namespace BloombergConsoleTester.Infrastructure.NHibernate
{
    public abstract class InstanceScoperBase<T> : IInstanceScoper<T>
    {
        public void ClearInstance(object key)
        {
            lock (GetDictionary().SyncRoot)
            {
                if (GetDictionary().Contains(key))
                {
                    RemoveInstance(key);
                }
            }
        }

        public void ClearScopedInstance(object key)
        {
            if (GetDictionary().Contains(key))
            {
                ClearInstance(key);
            }
        }

        public T GetScopedInstance(object key, Func<T> builder)
        {
            if (!GetDictionary().Contains(key))
            {
                BuildInstance(key, builder);
            }

            return (T)GetDictionary()[key];
        }

        protected abstract IDictionary GetDictionary();

        void AddInstance(object key, Func<T> builder)
        {
            T instance = builder.Invoke();
            GetDictionary().Add(key, instance);
        }

        void BuildInstance(object key, Func<T> builder)
        {
            lock (GetDictionary().SyncRoot)
            {
                if (!GetDictionary().Contains(key))
                {
                    AddInstance(key, builder);
                }
            }
        }

        void RemoveInstance(object key)
        {
            GetDictionary().Remove(key);
        }
    }
}