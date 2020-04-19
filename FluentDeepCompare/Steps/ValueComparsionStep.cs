using System;

namespace FluentDeepCompare.Steps
{
    internal static class ValueComparsionStep<T1, T2>
    {
        internal static ComparsionDelegate<T1, T2> Create(string name, Func<T1, object> leftGetValueFunc, Func<T2, object> rightGetValueFunc)
        {
            ComparsionResult Func(int level, T1 a, T2 b)
                => CompareFunc(name, leftGetValueFunc, rightGetValueFunc, level, a, b);

            return Func;
        }

        private static ComparsionResult CompareFunc(
            string name,
            Func<T1, object> leftGetValueFunc,
            Func<T2, object> rightGetValueFunc,
            int level, T1 leftObj, T2 rightObj)
        {
            var leftValue = (leftObj != null) ? leftGetValueFunc?.Invoke(leftObj) : null;
            var rightValue = (rightObj != null) ? rightGetValueFunc?.Invoke(rightObj) : null;

            bool areEqual = ValuesAreEqual(leftValue, rightValue);
            return new ComparsionResult(areEqual, ResultMessage.FormatTrackingMessage(name, level, '+', areEqual, leftValue, rightValue));
        }

        private static bool ValuesAreEqual(object left, object right)
        {
            bool equals;

            if (left is null && right is null)
                equals = true;
            else if (left is null || right is null)
                equals = false;
            else
                equals = left.Equals(right);

            return equals;
        }
    }
}