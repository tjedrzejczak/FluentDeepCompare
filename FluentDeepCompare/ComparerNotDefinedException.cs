using System;

namespace FluentDeepCompare
{
    [Serializable]
    public class ComparerNotDefinedException : Exception
    {
        private readonly Type _type1;
        private readonly Type _type2;

        public ComparerNotDefinedException(Type type1, Type type2)
            : base($"Comparer for {type1.FullName}{type2.FullName} not defined.")
        {
            _type1 = type1;
            _type2 = type2;
        }
    }
}