namespace FluentDeepCompare.Generation
{
    internal interface IComparersCache
    {
        void Add<TX, TY>(IComparer<TX, TY> comparer);
        IComparer<TX, TY> TryGet<TX, TY>();
        void Reserve<TX, TY>();
    }
}