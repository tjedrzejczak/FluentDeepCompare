# FluentDeepCompare

[![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/yprotech/FluentDeepCompare)](https://www.codefactor.io/repository/github/yprotech/FluentDeepCompare)
[![Nuget](https://img.shields.io/nuget/v/FluentDeepCompare)](https://www.nuget.org/packages/FluentDeepCompare)
[![GitHub](https://img.shields.io/github/repo-size/yprotech/FluentDeepCompare)](https://github.com/yprotech/FluentDeepCompare)
[![GitHub](https://img.shields.io/github/license/yprotech/FluentDeepCompare)](https://github.com/yprotech/FluentDeepCompare/blob/master/LICENSE)
![c](https://img.shields.io/badge/(c)-2020%20ypro.tech-blue)


FluentDeepCompare can be used to compare the content of objects of different types.
Objects are compared member by member, including other objects and collections.

FluentDeepCompare uses fluent configuration. The configuration can be global or prepared for specific comparison.

Example of global configuration:
```csharp
ObjectComparer.Configure(c => c
    .IncludePublicProperties()
    .IncludePublicFields()
);
```

Example of local configuration:
```csharp
var comparer = ObjectComparer.Create(useGlobal: true, c => c
    .IncludeCollections()
    .IncludeNotPublicFields()
    .IncludePublicFields()
    .Ignore<ClassE>(x => x.N1)
);
```

Using ``useGlobal: true`` copies the current global configuration as base of local configuration.


Comparison using global configuration:
```csharp
var result = ObjectComparer.Compare(objA, objB);
```

Comparison using local configuration:
```csharp
var result = comparer.Compare(objA, objB);
```

## Configuration

| Method (IConfigurator)          | Description                                | Default  |
|---------------------------------|--------------------------------------------|----------|
| UseOnlyMatchingMembers();       | Ignore members with different names        |          |
| IncludePublicProperties();      | Compare ``public`` properties              |  yes     |
| ExcludePublicProperties();      | Ignore ``public`` properties               |          |
| IncludeNotPublicProperties();   | Compare other than ``public`` properties   |          |
| ExcludeNotPublicProperties();   | Ignore other than ``public`` properties    |          |
| IncludePublicFields();          | Compare ``public`` fields                  |          |
| ExcludePublicFields();          | Ignore ``public`` fields                   |          |
| IncludeNotPublicFields();       | Compare other than ``public`` fields       |          |
| ExcludeNotPublicFields();       | Ignore other than ``public`` fields        |          |
| IncludeCollections();           | Compare collections                        |  yes     |
| ExcludeCollections();           | Ignore collections                         |          |
| WithMaxNestingLevel(int level); | Sets the maximum nesting level             |  10      |
| IgnoreType<TType>();            | Ignore object of type ``TType``            |          |
| Ignore<TType>(x => x.Member);   | Ignore member ``Member`` of type ``TType`` |          |
