using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentDeepCompare.Steps;

namespace FluentDeepCompare.Generation
{
    internal class StepsCreator<T1, T2>
    {
        private readonly IConfiguration _configuration;

        internal StepsCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal List<ComparsionDelegate<T1, T2>> GetSteps()
        {
            var members = DetectMembers();

            var filteredMembers = members.Values
                      .Where(FilterByOnlyMatchingMembers)
                      .Where(FilterCollections)
                      .Where(FilterIgnoredTypes)
                      .Where(FilterIgnoredMembers)
                      .ToList();

            return CreateSteps(filteredMembers);
        }

        private Dictionary<string, MatchingMemberInfo<T1, T2>> DetectMembers()
        {
            var lType = typeof(T1);
            var rType = typeof(T2);

            var members = new Dictionary<string, MatchingMemberInfo<T1, T2>>();

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            lType.GetProperties(bindingFlags).ToList().ForEach(p => AddProperty(members, p, MatchingType.Left));
            rType.GetProperties(bindingFlags).ToList().ForEach(p => AddProperty(members, p, MatchingType.Right));

            lType.GetFields(bindingFlags).ToList().ForEach(f => AddField(members, f, MatchingType.Left));
            rType.GetFields(bindingFlags).ToList().ForEach(f => AddField(members, f, MatchingType.Right));
            return members;
        }

        private List<ComparsionDelegate<T1, T2>> CreateSteps(List<MatchingMemberInfo<T1, T2>> members)
        {
            var steps = new List<ComparsionDelegate<T1, T2>>();

            steps.AddRange(
                members.Where(x => x.MatchingType == MatchingType.Left)
                    .Select(x => LeftShouldBeEmptyComparsionStep<T1, T2>.Create(x.Name, x.LeftSideItem.GetValueFunc))
            );

            steps.AddRange(
                members.Where(x => x.MatchingType == MatchingType.Right)
                    .Select(x => RightShouldBeEmptyComparsionStep<T1, T2>.Create(x.Name, x.RightSideItem.GetValueFunc))
            );

            steps.AddRange(
                members.Where(x => x.MatchingType == MatchingType.Both)
                    .Select(GetTwoSidedComparsionStep)
            );

            return steps;
        }

        private ComparsionDelegate<T1, T2> GetTwoSidedComparsionStep(MatchingMemberInfo<T1, T2> mmi)
        {
            if (mmi.BothSidesAreNotCollection)
            {
                if (mmi.LeftSideItem.CompareByValue && mmi.RightSideItem.CompareByValue)
                    return ValueComparsionStep<T1, T2>.Create(mmi.Name, mmi.LeftSideItem.GetValueFunc, mmi.RightSideItem.GetValueFunc);

                return ObjectComparsionStep<T1, T2>.Create(mmi, _configuration);
            }

            if (mmi.BothSidesAreCollections)
                return CollectionComparsionStep<T1, T2>.Create(mmi, _configuration);

            return CollectionToValueComparsionAlwaysFalseStep<T1, T2>.Create(mmi.Name);
        }

        private bool FilterByOnlyMatchingMembers(MatchingMemberInfo<T1, T2> mmi)
            => (_configuration.OnlyMatchingMembers)
                ? mmi.MatchingType == MatchingType.Both
                : mmi.MatchingType != MatchingType.None;

        private bool FilterCollections(MatchingMemberInfo<T1, T2> mmi)
            => (_configuration.UseCollections) || mmi.BothSidesAreNotCollection;

        private bool FilterIgnoredTypes(MatchingMemberInfo<T1, T2> mmi)
            => !((mmi.LeftSideItem != null && _configuration.IsTypeIgnored(mmi.LeftSideItem.MemberType))
                 || (mmi.RightSideItem != null && _configuration.IsTypeIgnored(mmi.RightSideItem.MemberType)));

        private bool FilterIgnoredMembers(MatchingMemberInfo<T1, T2> mmi)
            => !((mmi.LeftSideItem != null && _configuration.IsMemberIgnored(typeof(T1), mmi.Name))
                 || (mmi.RightSideItem != null && _configuration.IsMemberIgnored(typeof(T2), mmi.Name)));

        private void AddProperty(Dictionary<string, MatchingMemberInfo<T1, T2>> members,
            PropertyInfo pi,
            MatchingType matchingType)
        {
            if (pi.GetIndexParameters().Length > 0) //skip indexed property
                return;

            if (!members.ContainsKey(pi.Name))
                members.Add(pi.Name, new MatchingMemberInfo<T1, T2>(pi.Name));

            if (ShouldBeCompared(pi))
                members[pi.Name].Mark(matchingType, pi);
        }

        private void AddField(Dictionary<string, MatchingMemberInfo<T1, T2>> members,
            FieldInfo fi,
            MatchingType matchingType)
        {
            if (!members.ContainsKey(fi.Name))
                members.Add(fi.Name, new MatchingMemberInfo<T1, T2>(fi.Name));

            if (ShouldBeCompared(fi))
                members[fi.Name].Mark(matchingType, fi);
        }

        private bool ShouldBeCompared(PropertyInfo pi)
        {
            var getMethod = pi.GetGetMethod(true);

            if ((getMethod is null)                                                 // property without getter
                || (getMethod.IsPublic && !_configuration.UsePublicProperties)      // or public getter not allowed
                || (!getMethod.IsPublic && !_configuration.UseNotPublicProperties)) // or not public getter not allowed
                return false;

            return true;
        }

        private bool ShouldBeCompared(FieldInfo fi)
        {
            if ((fi.IsPublic && !_configuration.UsePublicFields)         // public field not allowed
                || (!fi.IsPublic && !_configuration.UseNotPublicFields)) // or not public field not allowed
                return false;

            return true;
        }
    }
}