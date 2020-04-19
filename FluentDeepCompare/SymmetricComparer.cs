namespace FluentDeepCompare
{
    internal class SymmetricComparer<T1, T2> : IComparer<T1, T2>
    {
        private readonly IComparer<T2, T1> _innerComparer;

        public SymmetricComparer(IComparer<T2, T1> innerComparer)
        {
            _innerComparer = innerComparer;
        }

        public ComparsionResult Compare(int level, T1 leftObj, T2 rightObj)
        {
            return _innerComparer.Compare(level, rightObj, leftObj);
        }
    }
}