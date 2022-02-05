# Cleaning Clean Architecture - PackIT Edition

The purpose of this repository is to investigate "Clean Architecture" and see if it can be improved upon. The goal is to remove complexity without reducing functionality.

To order to see the transformation incrementally, a branch has been created for each step. Simply compare the branch with the one before it to see the progression.


## Round 0 - Base State Validation

To being with we need to see if the application can be compiled and run. Likewise, we need to verify that all of the automated tests run.

There are only a handful of tests, so this was fairly easy. Just needed to change the connection string to a unique username and give is a password. Which is acceptable for a demonstration project.


## Round 1 - Fixing the Compiler Warnings

Consider this warning.

    CS0108	'PackingList.Id' hides inherited member 'AggregateRoot<PackingListId>.Id'. Use the new keyword if hiding was intended.	PackIT.Domain

That's pretty serious. 

```
    PackingList a = new PackingList();
    AggregateRoot<PackingListId> b = a;
    a.Id = Guid.NewGuid();
    Print(a.Id); //123e4567-e89b-12d3-a456-426614174000
    Print(b.Id); //00000000-0000-0000-0000-000000000000
```

This is going to be really confusing for whomever needs to troubleshoot why the `Id` is blank hald the time.

Fortunately the fix is as easy as just deleting the extraneous `Id` property from the `PackingList` class.

Then to prevent similar errors in the future, we're going add `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` to each csproj file.


## Round 2 - Detecting Unused Private Fields with Static Analysis

While looking at `PackingList` in Visual Studio, we notice that `Localization` is grayed out. Checking the compiler mesages, we see

```
IDE0052	Private member 'PackingList._localization' can be removed as the value assigned to it is never read	PackIT.Domain
```

Ok, let's do that. But before we do that, we should make it into a compiler error so no other unused fields slip into the code.

To do this, we create a `.editorconfig` file and place it in the `src` folder. By putting it here, all projects below it will inherit the same settings. This avoids the problem of having to copy and paste files into each project.

Rather than creating it from scratch, we used the "Add New Item" menu and selected the editor config file with ".NET" in its name. This adds a bunch of C# and VB sections with fairly reasonable defaults.  

Inside the `[*.{cs,vb}]` of the file we add these two lines.

```
dotnet_diagnostic.CA1823.severity=error
dotnet_diagnostic.IDE0052.severity=error
```

Rebuilding the project, we see that there is an unused parameter. So we'll have the compiler check for that as well.

```
dotnet_diagnostic.IDE0060.severity=error
```

In production grade code, there are two ways to handle static analysis.

1. Turn on the minimal rules at first. Then slowly add additional rules over time as problems are seen.
2. Turn on all the rules at first. Then remove the rules that are unreliable or not applicable.

## Round 3 - Merge the Shared Projects

A 'shared' project that could later be turned into a library that is reused across multiple applications is valuable. But arbitrarily dividing it into two projects just makes it hard to figure out where a given class is.

Furthermore, there is never a situation where an application would use one of the libraries without the other. So For now we'll merge `PackIT.Shared` and `PackIT.Shared.Abstractions`.

Long term, it may make sense to create subdivisions based on role such as `PackIT.Shared.Configuration` or `PackIT.Shared.Data`. But that is only necessary if some applications will only need a subset of the roles.


## Round 4 - Remove the Dispatcher Interfaces

The `InMemoryCommandDispatcher` class has a `ICommandDispatcher` interface. Likewise, the `InMemoryQueryDispatcher` class has a `IQueryDispatcher`. But why? They have no external dependnecies that would need to be mocked. Nor is there an alternative implementation that could be used in their place. 

So those interfaces need to go away. Things that don't benefit the code, that have no real purpose, should be removed.

And while we're at it, the classes will be renamed `CommandDispatcher` and `QueryDispatcher`. The prefix is not necessary as obviously they are "in-memory".




# PackIT
PackIT is simple "packing list app" built on top of clean architecture and CQRS.



