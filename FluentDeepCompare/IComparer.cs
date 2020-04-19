namespace FluentDeepCompare
{
    internal interface IComparer
    {
    }

    internal interface IComparer<in T1, in T2> : IComparer
    {
        ComparsionResult Compare(int level, T1 leftObj, T2 rightObj);
    }
}