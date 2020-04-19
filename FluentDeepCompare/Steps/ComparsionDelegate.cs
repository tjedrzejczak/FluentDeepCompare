namespace FluentDeepCompare.Steps
{
    public delegate ComparsionResult ComparsionDelegate<in T1, in T2>(int level, T1 leftOnj, T2 rightObj);
}