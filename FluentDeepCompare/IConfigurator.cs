using System;
using System.Linq.Expressions;

namespace FluentDeepCompare
{
    public interface IConfigurator
    {
        IConfigurator UseOnlyMatchingMembers();
        IConfigurator IncludePublicProperties();
        IConfigurator ExcludePublicProperties();
        IConfigurator IncludeNotPublicProperties();
        IConfigurator ExcludeNotPublicProperties();
        IConfigurator IncludePublicFields();
        IConfigurator ExcludePublicFields();
        IConfigurator IncludeNotPublicFields();
        IConfigurator ExcludeNotPublicFields();
        IConfigurator IncludeCollections();
        IConfigurator ExcludeCollections();

        IConfigurator WithMaxNestingLevel(int level);

        IConfigurator IgnoreType<T>();
        IConfigurator Ignore<TType>(Expression<Func<TType, object>> expression);
    }
}