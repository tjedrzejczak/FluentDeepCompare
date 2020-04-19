namespace FluentDeepCompare
{
    public interface IObjectComparer
    {
        ComparsionResult Compare<T1, T2>(T1 leftObj, T2 rightObj);
    }
}