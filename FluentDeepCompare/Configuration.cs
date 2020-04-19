using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentDeepCompare.Generation;

namespace FluentDeepCompare
{
    internal class Configuration : IConfiguration, IConfigurator
    {
        internal Configuration()
        {
        }

        internal Configuration(IConfiguration configuration)
        {
            OnlyMatchingMembers = configuration.OnlyMatchingMembers;
            UsePublicProperties = configuration.UsePublicProperties;
            UseNotPublicProperties = configuration.UseNotPublicProperties;
            UsePublicFields = configuration.UsePublicFields;
            UseNotPublicFields = configuration.UseNotPublicFields;
            UseCollections = configuration.UseCollections;
            MaxNestingLevel = configuration.MaxNestingLevel;

            _ignoredTypes = new HashSet<Type>(configuration.GetIgnoredTypes());
            _ignoredMembers = new HashSet<Tuple<Type, string>>(configuration.GetIgnoredMembers());
        }

        public bool OnlyMatchingMembers { get; private set; } = false;
        public bool UsePublicProperties { get; private set; } = true;
        public bool UseNotPublicProperties { get; private set; } = false;
        public bool UsePublicFields { get; private set; } = false;
        public bool UseNotPublicFields { get; private set; } = false;
        public bool UseCollections { get; private set; } = true;
        public int MaxNestingLevel { get; private set; } = 10;

        public IComparersCache Cache { get; } = new ComparersCache();

        protected readonly HashSet<Type> _ignoredTypes = new HashSet<Type>();
        protected readonly HashSet<Tuple<Type, string>> _ignoredMembers = new HashSet<Tuple<Type, string>>();

        public IConfigurator UseOnlyMatchingMembers()
        {
            OnlyMatchingMembers = true;
            return this;
        }

        public IConfigurator IncludePublicProperties()
        {
            UsePublicProperties = true;
            return this;
        }

        public IConfigurator ExcludePublicProperties()
        {
            UsePublicProperties = false;
            return this;
        }

        public IConfigurator IncludeNotPublicProperties()
        {
            UseNotPublicProperties = true;
            return this;
        }

        public IConfigurator ExcludeNotPublicProperties()
        {
            UseNotPublicProperties = true;
            return this;
        }

        public IConfigurator IncludePublicFields()
        {
            UsePublicFields = true;
            return this;
        }

        public IConfigurator ExcludePublicFields()
        {
            UsePublicFields = false;
            return this;
        }

        public IConfigurator IncludeNotPublicFields()
        {
            UseNotPublicFields = true;
            return this;
        }

        public IConfigurator ExcludeNotPublicFields()
        {
            UseNotPublicFields = false;
            return this;
        }

        public IConfigurator IncludeCollections()
        {
            UseCollections = true;
            return this;
        }

        public IConfigurator ExcludeCollections()
        {
            UseCollections = false;
            return this;
        }

        public IConfigurator WithMaxNestingLevel(int level)
        {
            MaxNestingLevel = level;
            return this;
        }

        public IConfigurator IgnoreType<T>()
        {
            var t = typeof(T);
            _ignoredTypes.Add(t);

            return this;
        }

        public IConfigurator Ignore<TType>(Expression<Func<TType, object>> expression)
        {
            if (expression?.Body.NodeType == ExpressionType.Convert && expression.Body is UnaryExpression unary)
            {
                if (unary.Operand.NodeType == ExpressionType.MemberAccess && unary.Operand is MemberExpression member1)
                {
                    AddIgnoredMember<TType>(member1.Member.Name);
                }
            }

            if (expression?.Body.NodeType == ExpressionType.MemberAccess && expression.Body is MemberExpression member2)
            {
                AddIgnoredMember<TType>(member2.Member.Name);
            }

            return this;
        }

        private void AddIgnoredMember<TType>(string name)
        {
            var key = new Tuple<Type, string>(typeof(TType), name);
            _ignoredMembers.Add(key);
        }

        bool IConfiguration.IsTypeIgnored(Type type)
        {
            var types = GetTypes(type);
            return types.Any(x => _ignoredTypes.Contains(x));
        }

        bool IConfiguration.IsMemberIgnored(Type type, string name)
        {
            var types = GetTypes(type);
            return types.Any(x => _ignoredMembers.Contains(new Tuple<Type, string>(x, name)));
        }

        List<Type> IConfiguration.GetIgnoredTypes()
            => _ignoredTypes.ToList();

        List<Tuple<Type, string>> IConfiguration.GetIgnoredMembers()
            => _ignoredMembers.ToList();

        private static IEnumerable<Type> GetTypes(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}