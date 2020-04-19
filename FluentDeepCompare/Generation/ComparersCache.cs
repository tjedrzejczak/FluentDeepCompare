using System;
using System.Collections.Generic;

namespace FluentDeepCompare.Generation
{
    internal class ComparersCache : IComparersCache
    {
        private readonly object _lock = new object();

        private readonly Dictionary<Tuple<Type, Type>, IComparer> _cache;

        internal ComparersCache()
        {
            _cache = new Dictionary<Tuple<Type, Type>, IComparer>();
        }

        void IComparersCache.Add<TX, TY>(IComparer<TX, TY> comparer)
        {
            lock (_lock)
            {
                var key = GetKey<TX, TY>();
                if (_cache.ContainsKey(key) && _cache[key] is DelayedComparer<TX, TY> deleyComparer)
                    deleyComparer.SetComparer(comparer);
                else
                    _cache.Add(key, comparer);
            }
        }

        IComparer<TX, TY> IComparersCache.TryGet<TX, TY>()
        {
            lock (_lock)
            {
                var key = GetKey<TX, TY>();
                if (_cache.ContainsKey(key))
                    return _cache[key] as IComparer<TX, TY>;
            }

            return null;
        }

        void IComparersCache.Reserve<TX, TY>()
        {
            lock (_lock)
            {
                var key = GetKey<TX, TY>();
                if (!_cache.ContainsKey(key))
                    _cache.Add(key, new DelayedComparer<TX, TY>());
            }
        }

        private static Tuple<Type, Type> GetKey<TX, TY>()
            => new Tuple<Type, Type>(typeof(TX), typeof(TY));
    }
}