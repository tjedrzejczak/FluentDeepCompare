using System;

namespace FluentDeepCompare.Generation
{
    [Flags]
    internal enum MatchingType
    {
        None = 0,
        Left = 1,
        Right = 2,

        Both = Left | Right
    }
}