using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentDeepCompare.Generation;

namespace FluentDeepCompare.Steps
{
    internal static class CollectionComparsionStep<T1, T2>
    {
        internal static ComparsionDelegate<T1, T2> Create(MatchingMemberInfo<T1, T2> mmi, IConfiguration configuration)
        {
            var factory = new ComparerFactory(configuration);
            var comparer = typeof(ComparerFactory)
                .GetMethod(nameof(ComparerFactory.Create), BindingFlags.Instance | BindingFlags.NonPublic)
                ?.MakeGenericMethod(mmi.LeftSideItem.CollectionItemType, mmi.RightSideItem.CollectionItemType)
                .Invoke(factory, null);

            var collectionItemCompareMethodInfo = comparer.GetType().GetMethod("Compare");

            ComparsionResult Func(int level, T1 a, T2 b)
                => CompareFunc(mmi.Name,
                    configuration.MaxNestingLevel,
                    mmi.LeftSideItem.GetValueFunc,
                    mmi.RightSideItem.GetValueFunc,
                    collectionItemCompareMethodInfo,
                    comparer,
                    level, a, b
                );

            return Func;
        }

        private static ComparsionResult CompareFunc(
            string name,
            int maxLevel,
            Func<T1, object> leftGetValueFunc,
            Func<T2, object> rightGetValueFunc,
            MethodInfo collectionItemCompareMethodInfo,
            object comparer,
            int level, T1 leftObj, T2 rightObj)
        {
            if (level >= maxLevel)
                return new ComparsionResult(true, $"Max level ({maxLevel}) occurred");

            List<ComparsionResult> stepResults = null;
            Exception exception = null;
            int? length = null;

            try
            {
                var leftValue = leftGetValueFunc?.Invoke(leftObj);
                var rightValue = rightGetValueFunc?.Invoke(rightObj);

                var leftCollection = ((System.Collections.IEnumerable)leftValue)?.Cast<object>().ToArray();
                var rightCollection = ((System.Collections.IEnumerable)rightValue)?.Cast<object>().ToArray();

                if (leftCollection != null && rightCollection != null)
                {
                    if (leftCollection.Length != rightCollection.Length)
                        return new ComparsionResult(false,
                            ResultMessage.CollecionsDiffLengthMessage(name, level, 'C', leftCollection.Length, rightCollection.Length));

                    length = leftCollection.Length;
                    stepResults = CompareCollectionItems(collectionItemCompareMethodInfo, comparer,
                        level, leftCollection, rightCollection).ToList();
                }
            } catch (Exception ex)
            {
                exception = ex;
            }

            return new ComparsionResult(ResultMessage.CollecionsLengthMessage(name, level, 'C', length), stepResults, exception);
        }

        private static IEnumerable<ComparsionResult> CompareCollectionItems(
            MethodInfo collectionItemCompareMethodInfo,
            object comparer,
            int level,
            object[] leftCollection,
            object[] rightCollection)
        {
            for (int i = 0; i < leftCollection.Length; i++)
            {
                var parameters = new[] { level + 1, leftCollection[i], rightCollection[i] };
                var ret = collectionItemCompareMethodInfo.Invoke(comparer, parameters);

                if (ret is ComparsionResult result)
                {
                    yield return result;
                }
            }
        }
    }
}