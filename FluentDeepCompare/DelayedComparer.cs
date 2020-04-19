namespace FluentDeepCompare
{
    internal class DelayedComparer<T1, T2> : IComparer<T1, T2>
    {
        private IComparer<T1, T2> _innerComparer;

        public ComparsionResult Compare(int level, T1 leftObj, T2 rightObj)
        {
            if (_innerComparer is null)
                throw new ComparerNotDefinedException(typeof(T1), typeof(T2));

            return _innerComparer.Compare(level, leftObj, rightObj);
        }

        internal void SetComparer(IComparer<T1, T2> comparer)
        {
            _innerComparer = comparer;
        }
    }
}