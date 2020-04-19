namespace FluentDeepCompare.Steps
{
    internal static class CollectionToValueComparsionAlwaysFalseStep<T1, T2>
    {
        internal static ComparsionDelegate<T1, T2> Create(string name)
        {
            ComparsionResult Func(int level, T1 a, T2 b)
                => new ComparsionResult(false, ResultMessage.FormatTrackingMessage(name, level, 'f', false, "", ""));

            return Func;
        }
    }
}