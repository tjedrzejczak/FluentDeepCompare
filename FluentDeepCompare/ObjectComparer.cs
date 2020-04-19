using System;

namespace FluentDeepCompare
{
    public static class ObjectComparer
    {
        private static readonly ConfiguredObjectComparer Global = new ConfiguredObjectComparer();

        public static IObjectComparer Create(Action<IConfigurator> configure)
            => Create(false, configure);

        public static IObjectComparer Create(bool useGlobal, Action<IConfigurator> configure)
        {
            var configuration = (useGlobal)
                ? Global.CloneConfiguration()
                : new Configuration();

            return new ConfiguredObjectComparer(configuration, configure);
        }

        public static void Configure(Action<IConfigurator> configure)
            => Global.Configure(configure);

        public static ComparsionResult Compare<T1, T2>(T1 leftObj, T2 rightObj)
            => Global.Compare(leftObj, rightObj);
    }
}