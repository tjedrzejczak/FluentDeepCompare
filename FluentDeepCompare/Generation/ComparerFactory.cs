namespace FluentDeepCompare.Generation
{
    internal class ComparerFactory
    {
        private readonly IConfiguration _configuration;

        public ComparerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal IComparer<T1, T2> Create<T1, T2>()
        {
            var comparer = _configuration.Cache.TryGet<T1, T2>();
            if (comparer != null)
                return comparer;

            var revComparer = _configuration.Cache.TryGet<T2, T1>();
            if (revComparer != null)
                return new SymmetricComparer<T1, T2>(revComparer);

            return CreateNew<T1, T2>();
        }

        private IComparer<T1, T2> CreateNew<T1, T2>()
        {
            var stepsCreator = new StepsCreator<T1, T2>(_configuration);

            _configuration.Cache.Reserve<T1, T2>();

            var comparer = new Comparer<T1, T2>(stepsCreator.GetSteps());

            _configuration.Cache.Add(comparer);
            return comparer;
        }
    }
}