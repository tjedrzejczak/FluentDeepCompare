using System;
using System.Collections.Generic;
using FluentDeepCompare.Generation;

namespace FluentDeepCompare
{
    internal interface IConfiguration
    {
        bool OnlyMatchingMembers { get; }
        bool UsePublicProperties { get; }
        bool UseNotPublicProperties { get; }
        bool UsePublicFields { get; }
        bool UseNotPublicFields { get; }
        bool UseCollections { get; }
        int MaxNestingLevel { get; }

        bool IsTypeIgnored(Type type);
        bool IsMemberIgnored(Type type, string name);

        List<Type> GetIgnoredTypes();
        List<Tuple<Type, string>> GetIgnoredMembers();

        IComparersCache Cache { get; } // scope of cache is the same as scope of configuration
    }
}