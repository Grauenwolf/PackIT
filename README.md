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



# PackIT
PackIT is simple "packing list app" built on top of clean architecture and CQRS.
