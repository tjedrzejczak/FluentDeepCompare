using System;

namespace FluentDeepCompare.Steps
{
    internal static class RightShouldBeEmptyComparsionStep<T1, T2>
    {
        internal static ComparsionDelegate<T1, T2> Create(string name, Func<T2, object> getValueFunc)
        {
            ComparsionResult Func(int level, T1 a, T2 b)
                => CompareFunc(name, getValueFunc, level, a, b);

            return Func;
        }

        private static ComparsionResult CompareFunc(string name, Func<T2, object> getValueFunc, int level, T1 leftObj, T2 rightObj)
        {
            var rightValue = getValueFunc?.Invoke(rightObj);
            bool areEqual = rightValue is null;
            return new ComparsionResult(areEqual, ResultMessage.FormatTrackingMessage(name, level, 'R', areEqual, "", rightValue));
        }
    }
}