using System;
using FluentDeepCompare.Generation;

namespace FluentDeepCompare
{
    internal class ConfiguredObjectComparer : IObjectComparer
    {
        private readonly Configuration _configuration;

        internal ConfiguredObjectComparer()
        {
            _configuration = new Configuration();
        }

        internal ConfiguredObjectComparer(Configuration baseConfiguration, Action<IConfigurator> configure)
        {
            _configuration = baseConfiguration;

            configure(_configuration);
        }

        internal Configuration CloneConfiguration()
            => new Configuration(_configuration);

        internal void Configure(Action<IConfigurator> configure)
            => configure(_configuration);

        public ComparsionResult Compare<T1, T2>(T1 leftObj, T2 rightObj)
        {
            var comparer = new ComparerFactory(_configuration).Create<T1, T2>();

            return comparer.Compare(0, leftObj, rightObj);
        }
    }
}