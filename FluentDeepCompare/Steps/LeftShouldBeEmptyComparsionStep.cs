using System;

namespace FluentDeepCompare.Steps
{
    internal class LeftShouldBeEmptyComparsionStep<T1, T2>
    {
        internal static ComparsionDelegate<T1, T2> Create(string name, Func<T1, object> getValueFunc)
        {
            ComparsionResult Func(int level, T1 a, T2 b)
                => CompareFunc(name, getValueFunc, level, a, b);

            return Func;
        }

        private static ComparsionResult CompareFunc(string name, Func<T1, object> getValueFunc, int level, T1 leftObj, T2 rightObj)
        {
            var leftValue = getValueFunc?.Invoke(leftObj);
            bool areEqual = leftValue is null;
            return new ComparsionResult(areEqual, ResultMessage.FormatTrackingMessage(name, level, 'L', areEqual, leftValue, ""));
        }
    }
}