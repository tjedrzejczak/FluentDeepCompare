using System;
using System.Collections.Generic;
using System.Linq;
using FluentDeepCompare.Steps;

namespace FluentDeepCompare
{
    internal class Comparer<T1, T2> : IComparer<T1, T2>
    {
        private readonly List<ComparsionDelegate<T1, T2>> _steps;

        internal Comparer(List<ComparsionDelegate<T1, T2>> steps)
        {
            _steps = steps;
        }

        public ComparsionResult Compare(int level, T1 leftObj, T2 rightObj)
        {
            List<ComparsionResult> stepResults = null;
            Exception exception = null;

            try
            {
                stepResults = _steps.Select(x => x.Invoke(level, leftObj, rightObj)).ToList();
            } catch (Exception ex)
            {
                exception = ex;
            }

            return new ComparsionResult(null, stepResults, exception);
        }
    }
}