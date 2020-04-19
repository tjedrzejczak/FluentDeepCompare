using System;
using System.Reflection;
using FluentDeepCompare.Generation;

namespace FluentDeepCompare.Steps
{
    internal static class ObjectComparsionStep<T1, T2>
    {
        internal static ComparsionDelegate<T1, T2> Create(MatchingMemberInfo<T1, T2> mmi, IConfiguration configuration)
        {
            var leftType = mmi.LeftSideItem.MemberType;
            var rightType = mmi.RightSideItem.MemberType;

            var factory = new ComparerFactory(configuration);
            var comparer = typeof(ComparerFactory)
                .GetMethod(nameof(ComparerFactory.Create), BindingFlags.Instance | BindingFlags.NonPublic)
                ?.MakeGenericMethod(leftType, rightType)
                .Invoke(factory, null);

            var itemCompareMethodInfo = comparer.GetType().GetMethod("Compare");

            ComparsionResult Func(int level, T1 a, T2 b)
                => CompareFunc(
                    mmi.Name,
                    configuration.MaxNestingLevel,
                    mmi.LeftSideItem.GetValueFunc,
                    mmi.RightSideItem.GetValueFunc,
                    leftType.Name,
                    rightType.Name,
                    itemCompareMethodInfo,
                    comparer,
                    level, a, b);

            return Func;
        }

        private static ComparsionResult CompareFunc(
                  string name,
                  int maxLevel,
                  Func<T1, object> leftGetValueFunc,
                  Func<T2, object> rightGetValueFunc,
                  string leftTypeName,
                  string rightTypeName,
                  MethodInfo itemCompareMethodInfo,
                  object comparer,
                  int level, T1 leftObj, T2 rightObj)
        {
            if (level >= maxLevel)
                return new ComparsionResult(true, $"Max level ({maxLevel}) occurred");

            Exception exception = null;

            try
            {
                var leftValue = leftGetValueFunc?.Invoke(leftObj);
                var rightValue = rightGetValueFunc?.Invoke(rightObj);

                if (leftValue is null && rightValue is null) // null == null
                    return new ComparsionResult(true, ResultMessage.FormatTrackingMessage(name, level, 'O', true, null, null));

                if (leftValue is null) // null != (not null)
                    return new ComparsionResult(false, ResultMessage.FormatTrackingMessage(name, level, 'O', false, null, rightTypeName));

                if (rightValue is null) // (not null) != null
                    return new ComparsionResult(false, ResultMessage.FormatTrackingMessage(name, level, 'O', false, leftTypeName, null));

                var parameters = new[] { level + 1, leftValue, rightValue };
                var ret = itemCompareMethodInfo.Invoke(comparer, parameters);

                if (ret is ComparsionResult result)
                {
                    return new ComparsionResult(ResultMessage.ObjectMessage(name, level, 'O'), new[] { result });
                }

            } catch (Exception ex)
            {
                exception = ex;
            }

            return new ComparsionResult(false, ResultMessage.FormatTrackingMessage(name, level, 'O', false, null, null), exception);
        }
    }
}