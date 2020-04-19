using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentDeepCompare.Generation
{
    internal class MatchingMemberInfo<T1, T2>
    {
        internal string Name { get; }
        internal MatchingType MatchingType { get; private set; } = MatchingType.None;

        internal MatchingMemberItem<T1> LeftSideItem { get; private set; }
        internal MatchingMemberItem<T2> RightSideItem { get; private set; }

        internal bool BothSidesAreCollections
            => LeftSideItem != null && LeftSideItem.IsCollection
                && RightSideItem != null && RightSideItem.IsCollection;

        internal bool BothSidesAreNotCollection
            => !((LeftSideItem != null && LeftSideItem.IsCollection)
                 ||
                 (RightSideItem != null && RightSideItem.IsCollection));

        internal MatchingMemberInfo(string name)
        {
            Name = name;
        }

        internal void Mark(MatchingType matchingType, PropertyInfo pi)
        {
            MatchingType |= matchingType;

            if (matchingType == MatchingType.Left)
                LeftSideItem = new PropertyMatchingMemberItem<T1>(pi);
            else if (matchingType == MatchingType.Right)
                RightSideItem = new PropertyMatchingMemberItem<T2>(pi);
        }

        internal void Mark(MatchingType matchingType, FieldInfo fi)
        {
            MatchingType |= matchingType;

            if (matchingType == MatchingType.Left)
                LeftSideItem = new FieldMatchingMemberItem<T1>(fi);
            else if (matchingType == MatchingType.Right)
                RightSideItem = new FieldMatchingMemberItem<T2>(fi);
        }
    }

    internal abstract class MatchingMemberItem<T>
    {
        internal Func<T, object> GetValueFunc;
        internal bool IsCollection;
        internal Type MemberType;
        internal Type CollectionItemType;

        public bool CompareByValue => MemberType.IsPrimitive || MemberType == typeof(string);

        protected void CheckCollection()
        {
            if (MemberType == typeof(string)) // String is not treated as collection of characters
                return;

            var collectionInterface = MemberType.GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (collectionInterface != null)
            {
                IsCollection = true;
                CollectionItemType = collectionInterface.GetGenericArguments()[0];
            }
        }
    }

    internal class PropertyMatchingMemberItem<T> : MatchingMemberItem<T>
    {
        internal PropertyMatchingMemberItem(PropertyInfo pi)
        {
            MemberType = pi.PropertyType;
            GetValueFunc = (obj) => pi.GetGetMethod()?.Invoke(obj, null);
            CheckCollection();
        }
    }

    internal class FieldMatchingMemberItem<T> : MatchingMemberItem<T>
    {
        public FieldMatchingMemberItem(FieldInfo fi)
        {
            MemberType = fi.FieldType;
            GetValueFunc = (obj) => fi.GetValue(obj);
            CheckCollection();
        }
    }
}